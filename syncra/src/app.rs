use std::sync::{Arc};
use winit::application::ApplicationHandler;
use winit::event::WindowEvent;
use winit::event_loop::{ActiveEventLoop};
use winit::window::{Window, WindowId};
use crate::renderer::Renderer;
use wgpu::hal::DynDevice;
use crate::state::State;

#[derive(Default)]
pub struct App<'a> {
    window: Option<Arc<Window>>,
    renderer: Option<Renderer<'a>>,
    state: Option<State>,
}

impl ApplicationHandler for App<'_> {
    fn resumed(&mut self, event_loop: &ActiveEventLoop) {
        if self.window.is_none() {
            let window = Arc::new(event_loop.create_window(Window::default_attributes()).unwrap());
            self.window = Some(window.clone());
        }
        if self.renderer.is_none() {
            let renderer = pollster::block_on(Renderer::new(self.window.as_ref().unwrap().clone()));
            self.renderer = Some(renderer);
        }
        if self.state.is_none() {
            let mut state = State::new();
            state.add_test_entity(); // temporary
            self.state = Some(state);
        }
    }

fn window_event(&mut self, event_loop: &ActiveEventLoop, window_id: WindowId, event: WindowEvent) {
        if window_id != self.window.as_ref().unwrap().id() {
            return;
        }

        match event {
            WindowEvent::CloseRequested => {
                event_loop.exit()
            },
            WindowEvent::Resized(physical_size) => {
                self.renderer.as_mut().unwrap().resize(physical_size);
            },
            WindowEvent::RedrawRequested => {
                self.state.as_mut().unwrap().update();
                self.renderer.as_mut().unwrap().render();
                self.window.as_mut().unwrap().request_redraw();
            },
            _ => {},
        }
    }

    fn suspended(&mut self, event_loop: &ActiveEventLoop) {
    }

    fn exiting(&mut self, event_loop: &ActiveEventLoop) {
    }
}