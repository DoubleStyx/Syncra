// state buffering/generation/rendering (might want to separate later)

use wgpu;

pub struct RenderContext {
    pub device: wgpu::Device,
    pub queue: wgpu::Queue,
}

impl RenderContext {
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

    pub fn render_loop(&mut self) {
        loop {
            let frame_state = session.wait_frame(openxr::Duration::INFINITE).unwrap();
            session.begin_frame().unwrap();
         
            for (view, swapchain) in views.iter().zip(&swapchains) {
                let swapchain_image = swapchain.acquire_image().unwrap();
                swapchain.wait_image(openxr::Duration::INFINITE).unwrap();
         
                let frame_texture = wgpu_device.create_texture(&wgpu::TextureDescriptor {
                    size: wgpu::Extent3d {
                        width: view.recommended_image_rect_width,
                        height: view.recommended_image_rect_height,
                        depth_or_array_layers: 1,
                    },
                    mip_level_count: 1,
                    sample_count: 1,
                    dimension: wgpu::TextureDimension::D2,
                    format: wgpu::TextureFormat::Bgra8Unorm,
                    usage: wgpu::TextureUsages::RENDER_ATTACHMENT,
                    label: Some("xr_frame_texture"),
                    ..Default::default()
                });
         
                let view = frame_texture.create_view(&wgpu::TextureViewDescriptor::default());
                let mut encoder = wgpu_device.create_command_encoder(&wgpu::CommandEncoderDescriptor {
                    label: Some("xr_frame_encoder"),
                });
         
                {
                    let mut render_pass = encoder.begin_render_pass(&wgpu::RenderPassDescriptor {
                        color_attachments: &[Some(wgpu::RenderPassColorAttachment {
                            view: &view,
                            resolve_target: None,
                            ops: wgpu::Operations {
                                load: wgpu::LoadOp::Clear(wgpu::Color::BLACK),
                                store: true,
                            },
                        })],
                        ..Default::default()
                    });
                    // Add rendering commands here
                }
         
                wgpu_queue.submit(Some(encoder.finish()));
         
                swapchain.release_image().unwrap();
            }
         
            session.end_frame(openxr::FrameEndInfo {
                predicted_display_time: frame_state.predicted_display_time,
                environment_blend_mode: EnvironmentBlendMode::OPAQUE,
                layers: &[],
            }).unwrap();
         }         
    }
}