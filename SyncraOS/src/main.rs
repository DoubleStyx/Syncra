use std::ffi::{c_int, CString};
use std::ptr;

#[allow(warnings)]
pub mod vulkan {
    include!(concat!(env!("OUT_DIR"), "/bindings/vulkan.rs"));
}
#[allow(warnings)]
pub mod glfw {
    include!(concat!(env!("OUT_DIR"), "/bindings/glfw.rs"));
}
#[allow(warnings)]
pub mod cglm {
    include!(concat!(env!("OUT_DIR"), "/bindings/cglm.rs"));
}
#[allow(warnings)]
pub mod openxr {
    include!(concat!(env!("OUT_DIR"), "/bindings/openxr.rs"));
}


pub fn main() {
    unsafe {
        if glfw::glfwInit() == glfw::GLFW_FALSE as i32 {
            eprintln!("Failed to initialize GLFW");
            return;
        }
    }
    
    unsafe { glfw::glfwWindowHint(glfw::GLFW_CLIENT_API as c_int, glfw::GLFW_NO_API as c_int); }
    unsafe { glfw::glfwWindowHint(glfw::GLFW_RESIZABLE as c_int, glfw::GLFW_FALSE as c_int); }
    
    let title = CString::new("Vulkan").expect("Failed to create CString");

    let window =
        unsafe {
            glfw::glfwCreateWindow(
                800,
                600,
                title.as_ptr(),
                ptr::null_mut(),
                ptr::null_mut(),
            )
        };

    unsafe {
        while glfw::glfwWindowShouldClose(window) == glfw::GLFW_FALSE as i32 {
            glfw::glfwPollEvents();
        }
    }
    
    // SyncraOS is the main driver and broker for Syncra. It drives the Vulkan renderer
    // and window/OpenXR context. It manages the lifecycles of various Syncra apps.
    // Each app is a separate C# process that may or may not be sandboxed.
    // You always start off in a local space, which is another C# process
    // with privileged permissions. This is essentially your desktop environment.
}