// state buffering/generation/rendering (might want to separate later)

use wgpu;
use crate::state::RENDERERS;

pub struct RenderContext {
    pub instance: wgpu::Instance,
    pub adapter: wgpu::Adapter,
    pub device: wgpu::Device,
    pub queue: wgpu::Queue,
}

impl RenderContext {
    pub async fn new(raw_window_handle: raw_window_handle::WindowHandle, raw_display_handle: raw_window_handle::DisplayHandle) -> Self {
        let instance = wgpu::Instance::new(wgpu::InstanceDescriptor {
            backends: wgpu::Backends::PRIMARY,
            ..Default::default()
        });

        let window = ; // create window from raw handles

        let surface = instance.create_surface(window).unwrap();

        let adapter = instance
            .request_adapter(&wgpu::RequestAdapterOptions {
                power_preference: wgpu::PowerPreference::default(),
                compatible_surface: Some(&surface),
                force_fallback_adapter: false,
            })
            .await
            .unwrap();

        let (device, queue) = adapter
            .request_device(
                &wgpu::DeviceDescriptor {
                    label: None,
                    required_features: wgpu::Features::empty(),
                    required_limits: if cfg!(target_arch = "wasm32") {
                        wgpu::Limits::downlevel_webgl2_defaults()
                    } else {
                        wgpu::Limits::default()
                    },
                    memory_hints: Default::default(),
                },
                None,
            )
            .await
            .unwrap();

        RenderContext {
            instance,
            adapter,
            device,
            queue
        }
    }

    pub fn update(&self)
    {

    }

    pub fn render(&self)
    {

    }

    pub fn render_loop(&mut self) {
        loop {
            self.update();
            self.render();
            // sleep thread
        }
    }
}