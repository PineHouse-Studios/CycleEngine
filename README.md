# Cycle Engine
> A scriptable 2D narrative engine for visual novels and dialogue-driven games.

Cycle Engine is a C# storytelling engine powered by **CES (Cycle Engine Script)** — a custom scripting language designed for writing cinematic scenes, branching dialogues, and character animations in plain text.

Cycle is engine-agnostic: it handles resource management, story logic, and animation state, while leaving rendering and platform-specific details to a host game engine. To run a Cycle project in Unity, Godot, or any other framework, implement the backend interfaces defined under `src/CycleEngine/Services` — examples are provided in the `examples/` directory.

## Concepts of this engine

### Cycle Engine Project Structure

### CES - Cycle Engine Script

### CDS - Cycle Dialog Script

### Entity System

### Command System

### Bootstrapper

### Script Executor
