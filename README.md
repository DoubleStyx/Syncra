# Syncra

A game-engine engine with a focus toward networked VR and desktop use cases. Isomorphic to a butterfly.

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

## Research Topics

- Put each world on separate thread?
- How to transition between worlds asynchronously? Transition during world updates?
- What ECS to use? Does it matter for networking? Write our own ECS?
- GUID table for mapping local entity IDs to networked IDs?
- Decouple Vulkan rendering from OpenXR/window context creation?
- Handles for interop boundary? Will this create lifetime issues when pooling these types?
- How to handle serialization efficiently? Serialization libraries?
- Codegen for handling serialization/networking boilerplate?
- Efficient data passing from managed to native side?
- Codegen for Rust types?
- How much marshalling will be needed?
- Do we work top-down, bottom-up, or meet-in-the-middle?
- What can be static? Do you ever need more than one engine? Probably not.
- How to simulate input without a full window? Input actions? Use input actions to drive world interaction events?
- Input system for abstracting input between the window and VR?
- Cross-thread logger for C#/Rust?