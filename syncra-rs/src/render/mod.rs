// state buffering/generation/rendering (might want to separate later)

use wgpu;

pub struct RenderContext {
    pub instance: wgpu::Instance,
    pub adapter: wgpu::Adapter,
}

impl RenderContext {
    pub async fn new() -> Self {
        let backend = wgpu::Backends::VULKAN;
        let instance_desc = wgpu::InstanceDescriptor {
            backends: backend,
            dx12_shader_compiler: Default::default(),
            ..Default::default()};
        let instance = wgpu::Instance::new(instance_desc);

        let request_adapter_options = wgpu::RequestAdapterOptions::default();

        let adapter = instance.request_adapter(&request_adapter_options).await.unwrap();

        Self {
            instance,
            adapter,
        }
    }

    pub fn render_loop(&mut self) {
        loop {

        }
    }
}