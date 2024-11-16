use winit::event_loop::{ControlFlow, EventLoop};
use crate::app::App;

mod app;
mod renderer;
mod state;
mod components;
mod systems;
mod assets;

pub fn main() {
    env_logger::init();

    let event_loop = EventLoop::new().unwrap();
    event_loop.set_control_flow(ControlFlow::Poll);

    let mut app = App::default();
    if let Err(e) = event_loop.run_app(&mut app) {
        eprintln!("Event loop error: {e}");
    }
}