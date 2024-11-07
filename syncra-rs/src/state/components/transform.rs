use bevy::ecs::component::Component;

#[derive(Component)]
struct Transform {
    position: Vec3,
    rotation: Vec3,
    scale: Vec3,
}