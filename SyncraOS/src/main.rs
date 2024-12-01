pub mod bindings {
    pub mod vulkan {
        include!(concat!(env!("OUT_DIR"), "/bindings/bindings_vulkan.rs"));
    }
    pub mod glfw {
        include!(concat!(env!("OUT_DIR"), "/bindings/bindings_glfw.rs"));
    }
    pub mod cglm {
        include!(concat!(env!("OUT_DIR"), "/bindings/bindings_cglm.rs"));
    }
    pub mod openxr {
        include!(concat!(env!("OUT_DIR"), "/bindings/bindings_openxr.rs"));
    }
}


pub fn main() {


    // SyncraOS is the main driver and broker for Syncra. It drives the Vulkan renderer
    // and window/OpenXR context. It manages the lifecycles of various Syncra apps.
    // Each app is a separate C# process that may or may not be sandboxed.
    // You always start off in a local space, which is another C# process
    // with privileged permissions. This is essentially your desktop environment.
}