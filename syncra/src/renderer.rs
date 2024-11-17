use std::error::Error;
use std::sync::Arc;
use winit::dpi::PhysicalSize;
use winit::window::Window;

pub struct Renderer<'a> {
    instance: wgpu::Instance,
    surface: wgpu::Surface<'a>,
}

impl<'a> Renderer<'a> {
    pub async fn new(window: &Arc<Window>) -> Result<Self, Box<dyn Error>> {
        let instance = wgpu::Instance::default();
        let surface = instance.create_surface(Arc::clone(&window)).unwrap();

        Ok( Self {
            instance,
            surface,
        })
    }

    pub fn resize(&mut self, new_size: PhysicalSize<u32>) {
        
    }
    
    pub fn update(&mut self) {
        
    }
    
    pub fn render(&mut self) {
        
    }
}