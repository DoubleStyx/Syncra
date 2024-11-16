use bevy_ecs::component::Component;
use nalgebra::Vector3;

#[derive(Component)]
pub struct Velocity {
    pub velocity: Vector3<f32>,
}