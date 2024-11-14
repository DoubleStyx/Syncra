use bevy_ecs::prelude::Component;
use nalgebra::Vector3;

#[derive(Component)]
pub struct Position { pub(crate) position: Vector3<f32> }
