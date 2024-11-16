use bevy_ecs::component::Component;
use nalgebra::Vector3;

#[derive(Component)]
pub struct Position { 
    pub position: Vector3<f32>,
}