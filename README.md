# Syncra

A game-engine engine with a focus toward networked VR and desktop use cases. Isomorphic to a butterfly.

The overall philosophy is to only use the minimum required libraries to conform to existing specifications. Anything that isn't a specification should be implemented directly.

The development strategy will be to heavily rely on existing libraries for prototyping and gradually prune them out with more custom implementations.

A Discord server is available for the project: https://discord.gg/bdVmAyGRH9

## Current Goals

- Networking
- VR and desktop support
- Efficient state synchronization
- Rendering
- ECS-style architecture
- Rust backend with C# scene logic
- Cross-platform support
- Standalone support
- Extensible architecture allowing different data models to be downloaded and used at runtime
- In-game scripting using a paradigm that has a graph-code isomorphism
- Physics
- Architectural inspirations from various topics in pure math, like type theory, category theory, and homotopy type theory
- In-game shader graphs, also with a graph-code isomorphism
- In-game asset creation tools
- Editor tools
- Multithreading
- Native plugin/mod support
- API and architecture documentation
- Asynchronous rendering
- Decentralized networking authority
- HDRP-like rendering specification

## Current Research Topics

- Website?
- What kind of ECS variant/style to use?
- Where to separate subsystems?
- How to handle build automation?
- CI/CD?
- Publishing platforms? Steam, Standalone, Oculus, Epic Games?
- Best way to pass data from managed side to renderer? Both will use ECS; how to efficiently handle interop boundary?
- Codegen for managed/native ECS?
- Should the ECS go on the Rust side? Rust protects against unsafe operations while remaining safe during highly concurrent workflows.
- Could I use Rust for the ECS internally while using C# for the high-level user interaction? My idea was to internally represent the scene using ECS but provide a higher-level abstraction for users looking at the scene graph.