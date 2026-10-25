# Cycle Engine

> An embeddable C# narrative engine for visual novels and dialogue-driven games.

Cycle Engine aims to make storytelling feel like writing ordinary C#, with a straightforward API for characters, dialogue, and choices.

Its design keeps narrative logic independent of rendering and platform details, so stories can run within different host engines.

## Concepts of this engine

### Cycle Engine Project Structure

### CES - Cycle Engine Script

### CDS - Cycle Dialog Script

### Entity System

### Command System

### Bootstrapper

### Script Executor

### C# Chapters and Source Generator

The C# script layer uses `[CycleChapter]` and `[CycleEvent]` with an incremental
source generator. See [the generator guide](docs/source-generator.md) for setup,
automatic event registration, partial-file ordering, and build checks.
