use wgpu;

pub struct render_context {
    pub device: wgpu::Device,
    pub queue: wgpu::Queue,
}

impl render_context {
    pub fn new() -> Self {
        let backend = wgpu::Backends::VULKAN;
        let instance_desc = wgpu::InstanceDescriptor {
            backends: backend,
            dx12_shader_compiler: Default::default(),
            ..Default::default()
        };
        let wgpu_instance = wgpu::Instance::new(instance_desc);
        
        let vk_instance = wgpu_instance.raw_instance().unwrap();
        let vk_binding = openxr::vulkan::GraphicsBindingVulkanKHR {
            instance: vk_instance.as_raw() as *mut _,
            physical_device: wgpu_physical_device.as_raw(),
            device: wgpu_device.as_raw(),
            queue_family_index: queue_family_index,
            queue_index: queue_index,
        };

        let session = instance
            .create_session(system, &vk_binding)
            .expect("Could not create OpenXR session");


        Self {
            device,
            queue,
        }
    }
}