// Run: dotnet run --project tests/GeneratorChecks
using System.Collections.Immutable;
using CycleEngine.Generators;
using CycleEngine.GeneratorSmoke;
using CycleEngine.Script.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

var checks = 0;
var chapter = new SplitChapter();
var config = chapter.ExportConfig();
Assert(config.ChapterId == "prologue" && config.EventCount == 3, "Chapter ID and event count");
Assert(config.ExecutionOrder.SequenceEqual(new[] { "Zebra", "Alpha", "Ending" }), "Cross-file source order");
foreach (var id in config.ExecutionOrder) config.EventRegistry[id]();
Assert(chapter.Calls.SequenceEqual(config.ExecutionOrder), "Private event invocation");
var another = new SplitChapter();
another.ExportConfig().EventRegistry["Zebra"]();
Assert(another.Calls.Count == 1 && chapter.Calls.Count == 3, "Instance-bound handlers");
Reject<NotSupportedException>(() => ((IList<string>)config.ExecutionOrder).Add("wrong"));
Reject<NotSupportedException>(() => ((IDictionary<string, Action>)config.EventRegistry).Add("wrong", () => { }));
var entries = new[] { new KeyValuePair<string, Action>("first", () => { }) };
var snapshot = new CycleScriptConfig("snapshot", entries);
entries[0] = new("changed", () => { });
Assert(snapshot.ExecutionOrder[0] == "first" && snapshot.EventRegistry.ContainsKey("first"), "Defensive config copy");
Reject<ArgumentException>(() => new CycleScriptConfig("duplicate", entries[0], entries[0]));
Reject<ArgumentException>(() => new CycleScriptConfig(" "));
Reject<ArgumentException>(() => new CycleScriptConfig("bad", new KeyValuePair<string, Action>("x", null!)));

var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp12);
var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator)
    .Append(typeof(CycleScript).Assembly.Location).Distinct().Select(path => MetadataReference.CreateFromFile(path)).ToArray();
const string imports = "using CycleEngine.Script.Attributes; using CycleEngine.Script.Types; ";
const string lifecycle = "public override void OnLoad() {} public override void OnExit() {} ";
var singleFile = Run(("Script.cs", imports + "[CycleChapter] public partial class Script : CycleScript { " + lifecycle + """
    [CycleEvent] private void Zebra() { }
    [CycleEvent] private void Alpha() { }
    }
    """));
NoErrors(singleFile);
var generated = Generated(singleFile);
Assert(generated.IndexOf("\"Zebra\"", StringComparison.Ordinal) < generated.IndexOf("\"Alpha\"", StringComparison.Ordinal), "Declaration order, not alphabetical order");

var splitSources = new[]
{
    ("Z.Part.cs", imports + "public partial class Split { [CycleEvent] void Tail() {} }"),
    ("A.Part.cs", imports + "[CycleChapter] public partial class Split : CycleScript { " + lifecycle + "[CycleEvent] void Head() {} }")
};
var split = Run(splitSources);
var reversed = Run(splitSources.Reverse().ToArray());
NoErrors(split);
NoErrors(reversed);
Assert(Generated(split) == Generated(reversed), "Partial order independent of compiler input order");
Assert(Generated(split).IndexOf("\"Head\"", StringComparison.Ordinal) < Generated(split).IndexOf("\"Tail\"", StringComparison.Ordinal), "Partial file path order");

var empty = Run(("Empty.cs", imports + "[CycleChapter] public partial class Empty : CycleScript { " + lifecycle + "}"));
NoErrors(empty);
Assert(Generated(empty).Contains("\"Empty\");", StringComparison.Ordinal), "Zero-event chapter");
var escaped = Run(("Keywords.cs", imports + "namespace @class { [CycleChapter(\"quoted\\\"chapter\")] public partial class @event : CycleScript { " + lifecycle + "[CycleEvent(\"event\\\"id\")] void @switch() {} } }"));
NoErrors(escaped);
Assert(Generated(escaped).Contains("this.@switch", StringComparison.Ordinal), "Escaped names and explicit IDs");

Expect("CYCLE002", imports + "[CycleChapter] public class Broken : CycleScript { " + lifecycle + "}");
Expect("CYCLE001", imports + "[CycleChapter] public partial class Broken { }");
Expect("CYCLE001", imports + "[CycleChapter] public partial class Broken<T> : CycleScript { " + lifecycle + "}");
Expect("CYCLE001", imports + "class Outer { [CycleChapter] public partial class Broken : CycleScript { " + lifecycle + "} }");
Expect("CYCLE001", imports + "[CycleChapter] file partial class Broken : CycleScript { " + lifecycle + "}");
Expect("CYCLE004", Chapter("[CycleEvent(\"same\")] void First() {} [CycleEvent(\"same\")] void Second() {}"));
Expect("CYCLE004", Chapter("[CycleEvent] void Same() {} [CycleEvent] void Same(int x) {}"));
Expect("CYCLE005", Chapter("public override CycleScriptConfig ExportConfig() => null!;"));
Expect("CYCLE006", imports + "class Orphan { [CycleEvent] void Event() {} }");
Expect("CYCLE007", Chapter("[CycleEvent(\" \u0020\")] void Event() {}"));
Expect("CYCLE007", imports + "[CycleChapter(\"\")] public partial class Broken : CycleScript { " + lifecycle + "}");
foreach (var method in new[]
{
    "static void Event() {}", "void Event(int x) {}", "int Event() => 0;",
    "void Event<T>() {}", "async void Event() { await System.Threading.Tasks.Task.Yield(); }",
    "System.Threading.Tasks.Task Event() => System.Threading.Tasks.Task.CompletedTask;",
    "abstract void Event();", "partial void Event();"
})
    Expect("CYCLE003", Chapter("[CycleEvent] " + method).Replace("public partial class", "public abstract partial class"));

var firstTree = Tree("Incremental.cs", Chapter("[CycleEvent] void First() {}"));
var compilation = Compilation(firstTree);
GeneratorDriver driver = CSharpGeneratorDriver.Create(
    generators: new[] { new CycleEventGenerator().AsSourceGenerator() },
    parseOptions: parseOptions,
    driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true));
driver = driver.RunGenerators(compilation);
var before = driver.GetRunResult().GeneratedTrees.Single().GetText().ToString();
driver = driver.RunGenerators(compilation.AddSyntaxTrees(Tree("Unrelated.cs", "class Unrelated { }")));
Assert(driver.GetRunResult().GeneratedTrees.Single().GetText().ToString() == before, "Unrelated edit preserves output");
Assert(driver.GetRunResult().Results.Single().TrackedOutputSteps.Values.SelectMany(steps => steps)
    .SelectMany(step => step.Outputs).All(output => output.Reason == IncrementalStepRunReason.Cached), "Unrelated edit caches source output");
var updated = compilation.ReplaceSyntaxTree(firstTree, Tree("Incremental.cs", Chapter("[CycleEvent] void Second() {} [CycleEvent] void First() {}")));
driver = driver.RunGenerators(updated);
Assert(driver.GetRunResult().GeneratedTrees.Single().GetText().ToString().Contains("\"Second\"", StringComparison.Ordinal), "Event edit regenerates output");
var splitCompilation = Compilation(splitSources.Select(source => Tree(source.Item1, source.Item2)).ToArray());
driver = CSharpGeneratorDriver.Create(new CycleEventGenerator()).WithUpdatedParseOptions(parseOptions).RunGenerators(splitCompilation);
var tailTree = splitCompilation.SyntaxTrees.Single(tree => tree.FilePath == "Z.Part.cs");
driver = driver.RunGenerators(splitCompilation.ReplaceSyntaxTree(tailTree, Tree("Z.Part.cs", imports + "public partial class Split { [CycleEvent] void ChangedTail() {} }")));
Assert(driver.GetRunResult().GeneratedTrees.Single().GetText().ToString().Contains("\"ChangedTail\"", StringComparison.Ordinal), "Unannotated partial edit regenerates output");

Console.WriteLine($"Generator checks passed ({checks} assertions). GeneratorSmoke compiled the real script layer for netstandard2.1.");

string Chapter(string body) => imports + "[CycleChapter] public partial class Broken : CycleScript { " + lifecycle + body + "}";
SyntaxTree Tree(string path, string source) => CSharpSyntaxTree.ParseText(source, parseOptions, path);
CSharpCompilation Compilation(params SyntaxTree[] trees) => CSharpCompilation.Create("GeneratorTest", trees, references,
    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
(GeneratorDriverRunResult Result, ImmutableArray<Diagnostic> Errors) Run(params (string Path, string Source)[] sources)
{
    GeneratorDriver testDriver = CSharpGeneratorDriver.Create(new CycleEventGenerator()).WithUpdatedParseOptions(parseOptions);
    testDriver = testDriver.RunGeneratorsAndUpdateCompilation(Compilation(sources.Select(source => Tree(source.Path, source.Source)).ToArray()), out var output, out var diagnostics);
    return (testDriver.GetRunResult(), diagnostics.Concat(output.GetDiagnostics()).Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).ToImmutableArray());
}
string Generated((GeneratorDriverRunResult Result, ImmutableArray<Diagnostic> Errors) result) => result.Result.GeneratedTrees.Single().GetText().ToString();
void NoErrors((GeneratorDriverRunResult Result, ImmutableArray<Diagnostic> Errors) result) =>
    Assert(result.Errors.Length == 0 && result.Result.Diagnostics.Length == 0,
        string.Join(Environment.NewLine, result.Result.Diagnostics.Concat(result.Errors)));
void Expect(string id, string source)
{
    var result = Run(("Invalid.cs", source));
    Assert(result.Result.Diagnostics.Any(diagnostic => diagnostic.Id == id), $"Expected {id}: {string.Join(Environment.NewLine, result.Errors)}");
    Assert(result.Result.Results.All(generator => generator.Exception is null), "Invalid input must not crash generator");
}
void Assert(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}
void Reject<T>(Action action) where T : Exception
{
    try { action(); }
    catch (T) { checks++; return; }
    throw new Exception($"Expected {typeof(T).Name}.");
}
