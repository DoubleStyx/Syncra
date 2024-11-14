use bevy_ecs::prelude::*;
use nalgebra::{Vector3, UnitQuaternion, Unit};
use crate::components::rotation::Rotation;
use crate::components::spinner::Spinner;

pub fn run(mut query: Query<(Entity, &mut Rotation, &mut Spinner)>) {
    for (entity, mut rotation, mut spinner) in query.iter_mut() {
        spinner.rotation_speed = Vector3::new(0.01, 0.0, 0.0);

        let rotation_quaternion = UnitQuaternion::from_euler_angles(
            spinner.rotation_speed.x,
            spinner.rotation_speed.y,
            spinner.rotation_speed.z,
        );

        rotation.quaternion = rotation.quaternion * rotation_quaternion;

        println!("{}", rotation.quaternion.to_string());
    }
}
