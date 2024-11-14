use bevy_ecs::prelude::Component;
use nalgebra::{Quaternion, UnitQuaternion};

#[derive(Component)]
pub struct Rotation { pub(crate) quaternion: UnitQuaternion<f32> }