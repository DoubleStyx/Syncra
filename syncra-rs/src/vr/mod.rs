use openxr as xr;
use std::ffi::CString;
use crate::render::RenderContext;
use crate::state::VR_CONTEXTS;


pub struct VRContext {
    render_context: RenderContext
}

impl VRContext {
    pub fn new_vr_context() -> u32 {
        let entry = xr::Entry::load();
        let app_info = xr::ApplicationInfo {
            application_name: "My OpenXR App",
            application_version: 1,
            engine_name: "My Engine",
            engine_version: 1,
        };



        let vk_app_info = xr::vulkan::ApplicationInfo {
            application_name: "Syncra",
            application_version: 0,
            engine_name: "Syncra",
            engine_version: 0,
        };

        let surface = ; // get openxr surface

        let render_context = RenderContext::new(surface);

        let vr_context = VRContext {
            render_context
        };

        let id = rand::random();

        VR_CONTEXTS.lock().unwrap().insert(id, vr_context);

        return id;
    }
}