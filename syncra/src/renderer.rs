use std::sync::{Arc};
use winit::{
    window::Window,
};
use winit::dpi::PhysicalSize;

pub struct Renderer<'a> {
    instance: wgpu::Instance,
    surface: wgpu::Surface<'a>,
}

impl<'a> Renderer<'a> {
    pub async fn new(window: Arc<Window>) -> Renderer<'a> {
        let instance = wgpu::Instance::default();
        let surface = instance.create_surface(Arc::clone(&window)).unwrap();

        Self {
            instance,
            surface,
        }
    }

    pub fn resize(&mut self, new_size: PhysicalSize<u32>) {
        println!("State resize")
    }

    pub fn draw(&self) {
        println!("State draw")
    }
}