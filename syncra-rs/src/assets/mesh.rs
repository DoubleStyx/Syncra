use bevy_asset::{Asset, Handle};
use nalgebra;
use nalgebra::{Vector2, Vector3, Vector4};
use bevy_reflect;
use bevy_reflect::Reflect;

pub struct Mesh {
    positions: Vec<Vector3<f32>>,
    normals: Vec<Vector3<f32>>,
    colors: Vec<Vector4<f32>>,
    uvs: Vec<Vector2<f32>>,
    indices: Vec<u32>,
}