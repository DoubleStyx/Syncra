# Syncra

A game-engine engine with a focus toward networked VR and desktop use cases. Isomorphic to a butterfly.

The overall philosophy is to only use the minimum required libraries to conform to existing specifications. Anything that isn't a specification should be implemented directly.

The development strategy will be to heavily rely on existing libraries for prototyping and gradually prune them out with more custom implementations.

A Discord server is available for the project: https://discord.gg/bdVmAyGRH9

## Current Goals

- Networking
- VR and desktop support
- Automatic state synchronization
- Advanced PBR renderer with realtime/baked lighting and ray-tracing support
- ECS-style architecture
- Rust backend with C# scene logic
- Cross-platform support
- Standalone support
- In-game scripting using a paradigm that has a graph-code isomorphism
- Advanced physics sims (fluid, cloth, softbody, etc.)
- In-game shader graphs, also with a graph-code isomorphism
- In-game asset creation tools
- Editor tools
- Multithreading
- Native plugin/mod support
- Asynchronous rendering
- Decentralized networking authority