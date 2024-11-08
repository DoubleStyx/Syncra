use bevy_ecs::prelude::*;
use nalgebra::{ Vector3, Quaternion};

#[derive(Component)]
pub struct MeshRendererComponent {
    pub mesh: Mesh,
    pub materials: Vec<Material>,
}