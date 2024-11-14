use bevy_ecs::prelude::Component;
use nalgebra::Vector3;

#[derive(Component)]
pub struct Spinner { pub(crate) rotation_speed: Vector3<f32> }