pub fn main() {
    // renderer, audio, input, openxr, and windowing all live here on the main thread
    // define an active/focused world by Guid
    // context switches happen as part of the Rust event loop
    // IPC using channels or shared memory/buffers
    // Rust process should manage/kill/open worlds dynamically
    // The worlds themselves are fully asynchronous
}