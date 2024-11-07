# Syncra

A game-engine engine with a focus toward networked VR and desktop use cases. Isomorphic to a butterfly.

The goal is to write everything using as few external libraries as possible, except where necessary to conform to existing standards for things like OpenXR, Vulkan, etc. All core functionality in the abstract should be written from scratch.

## Crates and Justification

- `winit`: Windowing and input. There aren't unified crates for handling windowing for each platform, but I might choose to use the platform-specific crates later on.
- `ash`: Vulkan bindings. The Vulkan API in Rust.
- `metal`: Metal bindings. The Metal API in Rust.
- `d3d12`: Direct3D 12 bindings. The Direct3D 12 API in Rust.
- `openxr`: OpenXR bindings. The OpenXR API in Rust.
- `windows`: Windows bindings. The Windows API in Rust.
- `coreaudio-sys`: CoreAudio bindings. The CoreAudio API in Rust.
- `alsa-sys`: ALSA bindings. The ALSA API in Rust.
