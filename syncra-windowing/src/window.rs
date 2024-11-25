use winit::event::{ElementState, Event, KeyEvent, WindowEvent};
use winit::event_loop::EventLoop;
use winit::keyboard::{KeyCode, PhysicalKey};
use winit::window::WindowBuilder;

pub struct Window {
    pub window: winit::window::Window,
}

impl Window {
    pub fn new(event_loop: &EventLoop<()>) -> Window {
        let window = WindowBuilder::new()
            .build(event_loop)
            .expect("Failed to create window");

        Self { window }
    }

    pub fn run(event_loop: EventLoop<()>, window: winit::window::Window) {
        event_loop
            .run(move |event, control_flow| match event {
                Event::WindowEvent {
                    ref event,
                    window_id,
                } if window_id == window.id() => match event {
                    WindowEvent::CloseRequested
                    | WindowEvent::KeyboardInput {
                        event:
                            KeyEvent {
                                state: ElementState::Pressed,
                                physical_key: PhysicalKey::Code(KeyCode::Escape),
                                ..
                            },
                        ..
                    } => control_flow.exit(),
                    _ => {}
                },
                _ => {}
            })
            .unwrap();
    }
}
