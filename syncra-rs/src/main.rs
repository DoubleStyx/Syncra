use bevy_ecs::schedule::Schedule;
use bevy_ecs::world::World;
pub mod xr;
pub mod window;
mod components;
mod systems;
mod resources;
mod rendering;
mod archetypes;

pub fn main() {
    let mut world = World::default();

    archetypes::spinner::new(&mut world);

    let mut schedule = Schedule::default();

    schedule.add_systems(systems::spinner::run);

    loop {
        schedule.run(&mut world);

        std::thread::sleep(std::time::Duration::from_millis(100));
    }
}