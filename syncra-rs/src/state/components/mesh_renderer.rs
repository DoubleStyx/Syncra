use bevy::ecs::component::Component;

#[derive(Component)]
struct MeshRenderer {
    mesh: Mesh,
}