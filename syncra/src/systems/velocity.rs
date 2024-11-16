use bevy_ecs::system::Query;
use crate::components::position::Position;
use crate::components::velocity::Velocity;

pub fn velocity(mut query: Query<(&mut Position, &Velocity)>) {
    for (mut position, velocity) in &mut query {
        position.position.x += velocity.velocity.x;
        position.position.y += velocity.velocity.y;
        position.position.z += velocity.velocity.z;
        
        println!("New position: {}", position.position); // temporary
    }
}