use bevy_ecs::prelude::*;
use nalgebra::{ Vector3, Quaternion};
use crate::assets::mesh::Mesh;
use crate::assets::material::Material;

#[derive(Component)]
pub struct MeshRendererComponent {
    pub mesh: Mesh,
    pub materials: Vec<Material>,
}