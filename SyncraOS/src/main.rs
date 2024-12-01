use std::ffi;
use std::ffi::c_char;
use ash::{vk as vk, Device};
use ash::{Entry, Instance};
use ash::ext::debug_utils;
use ash::khr::{surface, swapchain};
use glfw::{Action, Context, Key};
use raw_window_handle::{HasDisplayHandle, HasWindowHandle};

fn main() {
    let entry = Entry::linked();

    let app_name = unsafe { ffi::CStr::from_bytes_with_nul_unchecked(b"Syncra\0") };

    let appinfo = vk::ApplicationInfo::default()
        .application_name(app_name)
        .application_version(0)
        .engine_name(app_name)
        .engine_version(0)
        .api_version(vk::make_api_version(0, 1, 0, 0));

    let layer_names = [unsafe {
        ffi::CStr::from_bytes_with_nul_unchecked(
            b"VK_LAYER_KHRONOS_validation\0",
        )
    }];

    let layers_names_raw: Vec<*const c_char> = layer_names
        .iter()
        .map(|raw_name| raw_name.as_ptr())
        .collect();

    let mut glfw = glfw::init(glfw::fail_on_errors).unwrap();
    
    let window_width = 800;
    let window_height = 600;

    let (mut window, events) = glfw.create_window(window_width, window_height, "Hello this is window", glfw::WindowMode::Windowed)
        .expect("Failed to create GLFW window.");

    window.set_key_polling(true);
    window.make_current();

    println!(
        "Display Handle: {:?}, Window Handle: {:?}",
        window.display_handle().unwrap().as_raw(),
        window.window_handle().unwrap().as_raw()
    );

    let mut extension_names =
        ash_window::enumerate_required_extensions(window.display_handle().unwrap().as_raw())
            .unwrap()
            .to_vec();
    extension_names.push(debug_utils::NAME.as_ptr());

    for ext in &extension_names {
        println!("Extension: {:?}", unsafe { ffi::CStr::from_ptr(*ext) });
    }

    let create_flags = if cfg!(any(target_os = "macos", target_os = "ios")) {
        vk::InstanceCreateFlags::ENUMERATE_PORTABILITY_KHR
    } else {
        vk::InstanceCreateFlags::default()
    };

    let create_info = vk::InstanceCreateInfo::default()
        .application_info(&appinfo)
        .enabled_layer_names(&layers_names_raw)
        .enabled_extension_names(&extension_names)
        .flags(create_flags);

    let instance: Instance = unsafe {
        entry
            .create_instance(&create_info, None)
    }
        .expect("Instance creation error");


    let surface = unsafe {
        ash_window::create_surface(
            &entry,
            &instance,
            window.display_handle().unwrap().as_raw(),
            window.window_handle().unwrap().as_raw(),
            None,
        )
    }
        .unwrap();

    let pdevices = unsafe {
        instance
            .enumerate_physical_devices()
    }
        .expect("Physical device error");

    let surface_loader = surface::Instance::new(&entry, &instance);

    let (pdevice, queue_family_index) = pdevices
        .iter()
        .find_map(|pdevice| {
            unsafe { instance
                .get_physical_device_queue_family_properties(*pdevice) }
                .iter()
                .enumerate()
                .find_map(|(index, info)| {
                    let supports_graphic_and_surface =
                        info.queue_flags.contains(vk::QueueFlags::GRAPHICS)
                            && unsafe { surface_loader
                            .get_physical_device_surface_support(
                                *pdevice,
                                index as u32,
                                surface,
                            )}
                            .unwrap();
                    if supports_graphic_and_surface {
                        Some((*pdevice, index))
                    } else {
                        None
                    }
                })
        })
        .expect("Couldn't find suitable device.");

    let priorities = [1.0];

    let queue_info = vk::DeviceQueueCreateInfo::default()
        .queue_family_index(queue_family_index as u32)
        .queue_priorities(&priorities);

    let features = vk::PhysicalDeviceFeatures {
        shader_clip_distance: 1,
        ..Default::default()
    };

    let device_extension_names_raw = [
        swapchain::NAME.as_ptr(),
        #[cfg(any(target_os = "macos", target_os = "ios"))]
        ash::khr::portability_subset::NAME.as_ptr(),
    ];

    let device_create_info = vk::DeviceCreateInfo::default()
        .queue_create_infos(std::slice::from_ref(&queue_info))
        .enabled_extension_names(&device_extension_names_raw)
        .enabled_features(&features);

    let device: Device = unsafe {
        instance
            .create_device(pdevice, &device_create_info, None)
    }
        .unwrap();


    let swapchain_loader = swapchain::Device::new(&instance, &device);

    let surface_capabilities = unsafe {
        surface_loader
            .get_physical_device_surface_capabilities(pdevice, surface)
    }
        .unwrap();

    let desired_image_count = surface_capabilities.min_image_count + 1;

    let surface_format = unsafe {
        surface_loader
            .get_physical_device_surface_formats(pdevice, surface)
    }
        .unwrap()[0];

    let surface_resolution = match surface_capabilities.current_extent.width {
        u32::MAX => vk::Extent2D {
            width: window_width,
            height: window_height,
        },
        _ => surface_capabilities.current_extent,
    };

    let pre_transform = if surface_capabilities
        .supported_transforms
        .contains(vk::SurfaceTransformFlagsKHR::IDENTITY)
    {
        vk::SurfaceTransformFlagsKHR::IDENTITY
    } else {
        surface_capabilities.current_transform
    };

    let present_modes = unsafe {
        surface_loader
            .get_physical_device_surface_present_modes(pdevice, surface)
    }
        .unwrap();

    let present_mode = present_modes
        .iter()
        .cloned()
        .find(|&mode| mode == vk::PresentModeKHR::MAILBOX)
        .unwrap_or(vk::PresentModeKHR::FIFO);

    let swapchain_create_info = vk::SwapchainCreateInfoKHR::default()
        .surface(surface)
        .min_image_count(desired_image_count)
        .image_color_space(surface_format.color_space)
        .image_format(surface_format.format)
        .image_extent(surface_resolution)
        .image_usage(vk::ImageUsageFlags::COLOR_ATTACHMENT)
        .image_sharing_mode(vk::SharingMode::EXCLUSIVE)
        .pre_transform(pre_transform)
        .composite_alpha(vk::CompositeAlphaFlagsKHR::OPAQUE)
        .present_mode(present_mode)
        .clipped(true)
        .image_array_layers(1);

    let swapchain = unsafe {
        swapchain_loader
            .create_swapchain(&swapchain_create_info, None)
    }
        .unwrap();

    let pool_create_info = vk::CommandPoolCreateInfo::default()
        .flags(vk::CommandPoolCreateFlags::RESET_COMMAND_BUFFER)
        .queue_family_index(queue_family_index as u32);

    let pool = unsafe { device.create_command_pool(&pool_create_info, None) }.unwrap();

    while !window.should_close() {
        glfw.poll_events();
        for (_, event) in glfw::flush_messages(&events) {
            match event {
                glfw::WindowEvent::Key(Key::Escape, _, Action::Press, _) => {
                    window.set_should_close(true)
                }
                _ => {}
            }        }
    }

    // SyncraOS is the main driver and broker for Syncra. It drives the Vulkan renderer
    // and window/OpenXR context. It manages the lifecycles of various Syncra apps.
    // Each app is a separate C# process that may or may not be sandboxed.
    // You always start off in a local space, which is another C# process
    // with privileged permissions. This is essentially your desktop environment.
}