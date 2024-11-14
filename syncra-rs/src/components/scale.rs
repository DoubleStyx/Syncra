use bevy_ecs::prelude::Component;
use nalgebra::Vector3;

#[derive(Component)]
pub struct Scale { pub(crate) scale: Vector3<f32> }