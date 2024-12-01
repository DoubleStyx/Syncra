use std::ffi::CString;
use std::ptr;

pub mod bindings {
    pub mod vulkan {
        include!(concat!(env!("OUT_DIR"), "/bindings/vulkan.rs"));
    }
    pub mod glfw {
        include!(concat!(env!("OUT_DIR"), "/bindings/glfw.rs"));
    }
    pub mod cglm {
        include!(concat!(env!("OUT_DIR"), "/bindings/cglm.rs"));
    }
    pub mod openxr {
        include!(concat!(env!("OUT_DIR"), "/bindings/openxr.rs"));
    }
}


pub fn main() {
    unsafe {
        if bindings::glfw::glfwInit() == bindings::glfw::GLFW_FALSE as i32 {
            eprintln!("Failed to initialize GLFW");
            return;
        }
    }

    let window_title = CString::new("Hello GLFW").expect("Failed to create CString");
    let window = unsafe {
        bindings::glfw::glfwCreateWindow(
            640,
            480,
            window_title.as_ptr(),
            ptr::null_mut(),
            ptr::null_mut(),
        )
    };

    if window.is_null() {
        eprintln!("Failed to create GLFW window");
        unsafe { bindings::glfw::glfwTerminate(); }
        return;
    }

    unsafe { bindings::glfw::glfwMakeContextCurrent(window); }

    unsafe { bindings::glfw::glfwSwapInterval(1); }

    unsafe {
        while bindings::glfw::glfwWindowShouldClose(window) == bindings::glfw::GLFW_FALSE as i32 {
            bindings::glfw::glClear(bindings::glfw::GL_COLOR_BUFFER_BIT);

            bindings::glfw::glfwSwapBuffers(window);

            bindings::glfw::glfwPollEvents();
        }
    }

    unsafe { bindings::glfw::glfwDestroyWindow(window); }
    unsafe { bindings::glfw::glfwTerminate(); }

    

    // SyncraOS is the main driver and broker for Syncra. It drives the Vulkan renderer
    // and window/OpenXR context. It manages the lifecycles of various Syncra apps.
    // Each app is a separate C# process that may or may not be sandboxed.
    // You always start off in a local space, which is another C# process
    // with privileged permissions. This is essentially your desktop environment.
}