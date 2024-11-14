use bevy_ecs::prelude::*;

#[derive(Resource, Default)]
struct Texture {
    texture: ash::vk::Image,
}
