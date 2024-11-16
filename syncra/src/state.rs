use bevy_ecs::schedule::Schedule;
use bevy_ecs::world::World;
use nalgebra::Vector3;
use crate::components::position::Position;
use crate::components::velocity::Velocity;
use crate::systems::velocity::velocity;

pub struct State {
    world: World,
    schedule: Schedule
}

impl State {
    pub fn new() -> State {
        let world = World::default();
        let mut schedule = Schedule::default();

        schedule.add_systems(velocity);
        
        State { 
            world,
            schedule
        }
    }
    
    pub fn add_test_entity(&mut self)
    {
        self.world.spawn((
            Position { position: Vector3::new(1.0, 1.0, 1.0) },
            Velocity { velocity: Vector3::new(1.0, -1.0, 0.5) },
        ));
    }
    
    pub fn update(&mut self) {
        self.schedule.run(&mut self.world);
    }
}