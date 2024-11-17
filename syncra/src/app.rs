use std::sync::Arc;
use pollster::block_on;
use winit::application::ApplicationHandler;
use winit::event::WindowEvent;
use winit::event_loop::ActiveEventLoop;
use winit::window::{Window, WindowId};
use crate::renderer::Renderer;

#[derive(Default)]
pub struct App<'a> {
    window: Option<Arc<Window>>,
    renderer: Option<Renderer<'a>>,
}

impl ApplicationHandler for App<'_> {
    fn resumed(&mut self, event_loop: &ActiveEventLoop) {
        println!("App resumed");
        if self.window.is_none() {
            let window = Arc::new(event_loop.create_window(Window::default_attributes()).unwrap());
            self.window = Some(window.clone());
        }    
        
        if self.renderer.is_none() {
            let renderer = block_on(Renderer::new(self.window.as_ref().unwrap())).unwrap();
            self.renderer = Some(renderer);
        }
    }

    fn window_event(&mut self, event_loop: &ActiveEventLoop, window_id: WindowId, event: WindowEvent) {
        if window_id != self.window.as_ref().unwrap().id() {
            return;
        }

        match event {
            WindowEvent::CloseRequested => {
                println!("Close requested");
                event_loop.exit()
            },
            WindowEvent::Resized(physical_size) => {
                println!("Resize requested");
                self.renderer.as_mut().unwrap().resize(physical_size);
            },
            WindowEvent::RedrawRequested => {
                println!("Redraw requested");
                self.renderer.as_mut().unwrap().render();
                self.window.as_ref().unwrap().request_redraw();
            },
            _ => {},
        }    }
}