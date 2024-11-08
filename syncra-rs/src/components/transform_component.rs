use bevy_ecs::prelude::*;
use nalgebra::{ Vector3, Quaternion};

#[derive(Component)]
pub struct TransformComponent {
    pub position: Vector3<f32>,
    pub rotation: Quaternion<f32>,
    pub scale: Vector3<f32>,
}