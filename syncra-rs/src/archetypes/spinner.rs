use bevy_ecs::world::World;
use crate::components::position::Position;
use crate::components::rotation::Rotation;
use nalgebra::{Vector3, Quaternion, UnitQuaternion};
use crate::components::scale::Scale;
use crate::components::spinner::Spinner;

pub fn new(world: &mut World) {
    let entity = world.spawn((
        Position { position: Vector3::new(0.0, 0.0, 0.0) },
        Rotation { quaternion: UnitQuaternion::new(Vector3::new(0.0, 0.0, 0.0)) },
        Scale { scale: Vector3::new(0.5, 0.5, 0.5) },
        Spinner { rotation_speed: Vector3::new(0.0, 0.0, 0.0) },
    ));
}
