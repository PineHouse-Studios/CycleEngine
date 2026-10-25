using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CycleEngine.Generators;

[Generator(LanguageNames.CSharp)]
public sealed class CycleEventGenerator : IIncrementalGenerator
{
    private const string ChapterAttribute = "CycleEngine.Script.Attributes.CycleChapterAttribute";
    private const string EventAttribute = "CycleEngine.Script.Attributes.CycleEventAttribute";
    private const string ScriptType = "CycleEngine.Script.Types.CycleScript";

    private static readonly DiagnosticDescriptor InvalidChapter = Rule("CYCLE001",
        "Invalid chapter", "Chapter '{0}' must be a non-record, non-generic, non-nested class derived from CycleScript, without the file modifier");
    private static readonly DiagnosticDescriptor NeedsPartial = Rule("CYCLE002",
        "Chapter must be partial", "Every declaration of chapter '{0}' must have the partial modifier");
    private static readonly DiagnosticDescriptor InvalidEvent = Rule("CYCLE003",
        "Invalid event method", "Event '{0}' must be a concrete, non-generic instance method returning void, with no parameters, async modifier, or partial declaration");
    private static readonly DiagnosticDescriptor DuplicateId = Rule("CYCLE004",
        "Duplicate event ID", "Chapter '{0}' contains more than one event with ID '{1}'");
    private static readonly DiagnosticDescriptor ExportConflict = Rule("CYCLE005",
        "ExportConfig is generated", "Remove the manually declared ExportConfig member from chapter '{0}'; the generator supplies it");
    private static readonly DiagnosticDescriptor MissingChapter = Rule("CYCLE006",
        "Event requires a chapter", "Event '{0}' must be declared in a class marked with [CycleChapter]");
    private static readonly DiagnosticDescriptor EmptyId = Rule("CYCLE007",
        "Empty ID", "The explicit ID for '{0}' must not be empty or whitespace");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var chapters = context.SyntaxProvider.ForAttributeWithMetadataName(
            ChapterAttribute,
            static (node, _) => node is TypeDeclarationSyntax,
            static (attributeContext, cancellation) => Generate(attributeContext, cancellation));

        context.RegisterSourceOutput(chapters, static (output, chapter) =>
        {
            foreach (var diagnostic in chapter.Diagnostics)
                output.ReportDiagnostic(diagnostic);
            if (chapter.Source is not null)
                output.AddSource(chapter.HintName, SourceText.From(chapter.Source, Encoding.UTF8));
        });

        var orphanEvents = context.SyntaxProvider.ForAttributeWithMetadataName(
            EventAttribute,
            static (node, _) => node is MethodDeclarationSyntax,
            static (attributeContext, _) =>
            {
                var method = (IMethodSymbol)attributeContext.TargetSymbol;
                return FindAttribute(method.ContainingType, ChapterAttribute) is null
                    ? Diagnostic.Create(MissingChapter, attributeContext.TargetNode.GetLocation(), method.Name)
                    : null;
            }).Where(static diagnostic => diagnostic is not null);
        context.RegisterSourceOutput(orphanEvents, static (output, diagnostic) => output.ReportDiagnostic(diagnostic!));
    }

    private static GenerationResult Generate(GeneratorAttributeSyntaxContext context, CancellationToken cancellation)
    {
        var chapter = (INamedTypeSymbol)context.TargetSymbol;
        var diagnostics = ImmutableArray.CreateBuilder<Diagnostic>();
        var location = context.TargetNode.GetLocation();
        var declarations = chapter.DeclaringSyntaxReferences
            .Select(reference => reference.GetSyntax(cancellation)).OfType<TypeDeclarationSyntax>().ToArray();

        if (chapter.TypeKind != TypeKind.Class || chapter.IsRecord || chapter.Arity != 0 ||
            chapter.ContainingType is not null || !DerivesFromScript(chapter) ||
            declarations.Any(declaration => declaration.Modifiers.Any(SyntaxKind.FileKeyword)))
            diagnostics.Add(Diagnostic.Create(InvalidChapter, location, chapter.Name));

        if (declarations.Any(declaration => !declaration.Modifiers.Any(SyntaxKind.PartialKeyword)))
            diagnostics.Add(Diagnostic.Create(NeedsPartial, location, chapter.Name));

        if (chapter.GetMembers("ExportConfig").Length != 0)
            diagnostics.Add(Diagnostic.Create(ExportConflict, location, chapter.Name));

        var chapterId = ReadId(context.Attributes[0], chapter.Name);
        if (string.IsNullOrWhiteSpace(chapterId))
            diagnostics.Add(Diagnostic.Create(EmptyId, location, chapter.Name));

        var events = new List<(string Id, string Method, string Path, int Position)>();
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var method in chapter.GetMembers().OfType<IMethodSymbol>())
        {
            cancellation.ThrowIfCancellationRequested();
            var attribute = FindAttribute(method, EventAttribute);
            if (attribute is null) continue;
            var syntax = (MethodDeclarationSyntax)method.DeclaringSyntaxReferences[0].GetSyntax(cancellation);
            var eventLocation = syntax.GetLocation();
            if (method.MethodKind != MethodKind.Ordinary || method.IsStatic || method.IsAbstract ||
                method.IsExtern || method.IsGenericMethod || !method.ReturnsVoid || method.Parameters.Length != 0 ||
                method.IsAsync || syntax.Modifiers.Any(SyntaxKind.PartialKeyword))
                diagnostics.Add(Diagnostic.Create(InvalidEvent, eventLocation, method.Name));

            var id = ReadId(attribute, method.Name);
            if (string.IsNullOrWhiteSpace(id))
                diagnostics.Add(Diagnostic.Create(EmptyId, eventLocation, method.Name));
            if (!ids.Add(id))
                diagnostics.Add(Diagnostic.Create(DuplicateId, eventLocation, chapter.Name, id));
            events.Add((id, method.Name, syntax.SyntaxTree.FilePath.Replace('\\', '/'), syntax.SpanStart));
        }

        var hintName = chapter.ToDisplayString().Replace("@", "") + ".CycleEvents.g.cs";
        if (diagnostics.Count != 0)
            return new GenerationResult(hintName, null, diagnostics.ToImmutable());

        // C# defines no declaration order between partial files; make it independent of compiler input order.
        var ordered = events.OrderBy(item => item.Path, StringComparer.Ordinal).ThenBy(item => item.Position);
        var source = new StringBuilder("// <auto-generated/>\n#nullable enable\n");
        var hasNamespace = !chapter.ContainingNamespace.IsGlobalNamespace;
        if (hasNamespace)
            source.Append("namespace ").Append(chapter.ContainingNamespace.ToDisplayString()).AppendLine("\n{");
        source.Append("partial class @").Append(chapter.Name).AppendLine("\n{");
        source.AppendLine("    public override global::CycleEngine.Script.Types.CycleScriptConfig ExportConfig()");
        source.AppendLine("    {");
        source.AppendLine("        return new global::CycleEngine.Script.Types.CycleScriptConfig(");
        source.Append("            ").Append(SymbolDisplay.FormatLiteral(chapterId, true));
        foreach (var item in ordered)
        {
            source.AppendLine(",");
            source.Append("            new global::System.Collections.Generic.KeyValuePair<string, global::System.Action>(")
                .Append(SymbolDisplay.FormatLiteral(item.Id, true)).Append(", this.@").Append(item.Method).Append(')');
        }
        source.AppendLine(");\n    }\n}");
        if (hasNamespace) source.AppendLine("}");
        return new GenerationResult(hintName, source.ToString(), diagnostics.ToImmutable());
    }

    private static bool DerivesFromScript(INamedTypeSymbol type)
    {
        for (var baseType = type.BaseType; baseType is not null; baseType = baseType.BaseType)
            if (baseType.ToDisplayString() == ScriptType) return true;
        return false;
    }

    private static AttributeData? FindAttribute(ISymbol symbol, string name) =>
        symbol.GetAttributes().FirstOrDefault(attribute => attribute.AttributeClass?.ToDisplayString() == name);

    private static string ReadId(AttributeData attribute, string fallback) =>
        attribute.ConstructorArguments.Length > 0 && attribute.ConstructorArguments[0].Value is string id ? id : fallback;

    private static DiagnosticDescriptor Rule(string id, string title, string message) =>
        new(id, title, message, "CycleEngine.Generation", DiagnosticSeverity.Error, isEnabledByDefault: true);

    private sealed class GenerationResult : IEquatable<GenerationResult>
    {
        public GenerationResult(string hintName, string? source, ImmutableArray<Diagnostic> diagnostics)
        {
            HintName = hintName;
            Source = source;
            Diagnostics = diagnostics;
        }

        public string HintName { get; }
        public string? Source { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }

        public bool Equals(GenerationResult? other) => other is not null && HintName == other.HintName &&
            Source == other.Source && Diagnostics.SequenceEqual(other.Diagnostics);

        public override bool Equals(object? obj) => obj is GenerationResult other && Equals(other);
        public override int GetHashCode() => HintName.GetHashCode();
    }
}
