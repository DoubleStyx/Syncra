use std::ffi;
use std::ffi::c_char;
use std::io::Cursor;
use std::mem::offset_of;
use ash::{vk as vk, Device};
use ash::{Entry, Instance};
use ash::ext::debug_utils;
use ash::khr::{surface, swapchain};
use ash::util::read_spv;
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

    glfw.window_hint(glfw::WindowHint::ClientApi(glfw::ClientApiHint::NoApi));

    let window_width = 800;
    let window_height = 600;

    let (mut window, events) = glfw.create_window(window_width, window_height, "Syncra", glfw::WindowMode::Windowed)
        .expect("Failed to create GLFW window.");

    let mut extension_names =
        ash_window::enumerate_required_extensions(window.display_handle().unwrap().as_raw())
            .unwrap()
            .to_vec();
    extension_names.push(debug_utils::NAME.as_ptr());

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

    let command_buffer_allocate_info = vk::CommandBufferAllocateInfo::default()
        .command_buffer_count(2)
        .command_pool(pool)
        .level(vk::CommandBufferLevel::PRIMARY);

    let command_buffers = unsafe {
        device
            .allocate_command_buffers(&command_buffer_allocate_info)
    }
        .unwrap();

    let setup_command_buffer = command_buffers[0];
    let draw_command_buffer = command_buffers[1];

    let semaphore_create_info = vk::SemaphoreCreateInfo::default();

    let present_complete_semaphore = unsafe {
        device
            .create_semaphore(&semaphore_create_info, None)
    }
        .unwrap();
    let rendering_complete_semaphore = unsafe {
        device
            .create_semaphore(&semaphore_create_info, None)
    }
        .unwrap();

    let viewports = [vk::Viewport {
        x: 0.0,
        y: 0.0,
        width: surface_resolution.width as f32,
        height: surface_resolution.height as f32,
        min_depth: 0.0,
        max_depth: 1.0,
    }];
    let scissors = [surface_resolution.into()];

    let viewport_state_info = vk::PipelineViewportStateCreateInfo::default()
        .scissors(&scissors)
        .viewports(&viewports);

    let mut vertex_spv_file =
        Cursor::new(&include_bytes!("../../shader/triangle/vert.spv")[..]);
    let mut frag_spv_file = Cursor::new(&include_bytes!("../../shader/triangle/frag.spv")[..]);

    let vertex_code =
        read_spv(&mut vertex_spv_file).expect("Failed to read vertex shader spv file");
    let vertex_shader_info = vk::ShaderModuleCreateInfo::default().code(&vertex_code);

    let frag_code =
        read_spv(&mut frag_spv_file).expect("Failed to read fragment shader spv file");
    let frag_shader_info = vk::ShaderModuleCreateInfo::default().code(&frag_code);

    let vertex_shader_module = unsafe {
        device
            .create_shader_module(&vertex_shader_info, None)
    }
        .expect("Vertex shader module error");

    let fragment_shader_module = unsafe {
        device
            .create_shader_module(&frag_shader_info, None)
    }
        .expect("Fragment shader module error");

    let shader_entry_name = unsafe { ffi::CStr::from_bytes_with_nul_unchecked(b"main\0") };

    let shader_stage_create_infos = [
        vk::PipelineShaderStageCreateInfo {
            module: vertex_shader_module,
            p_name: shader_entry_name.as_ptr(),
            stage: vk::ShaderStageFlags::VERTEX,
            ..Default::default()
        },
        vk::PipelineShaderStageCreateInfo {
            s_type: vk::StructureType::PIPELINE_SHADER_STAGE_CREATE_INFO,
            module: fragment_shader_module,
            p_name: shader_entry_name.as_ptr(),
            stage: vk::ShaderStageFlags::FRAGMENT,
            ..Default::default()
        },
    ];

    let vertex_input_attribute_descriptions = [
        vk::VertexInputAttributeDescription {
            location: 0,
            binding: 0,
            format: vk::Format::R32G32B32A32_SFLOAT,
            offset: offset_of!(Vertex, pos) as u32,
        },
        vk::VertexInputAttributeDescription {
            location: 1,
            binding: 0,
            format: vk::Format::R32G32B32A32_SFLOAT,
            offset: offset_of!(Vertex, color) as u32,
        },
    ];

    let vertex_input_binding_descriptions = [vk::VertexInputBindingDescription {
        binding: 0,
        stride: size_of::<Vertex>() as u32,
        input_rate: vk::VertexInputRate::VERTEX,
    }];

    let vertex_input_state_info = vk::PipelineVertexInputStateCreateInfo::default()
        .vertex_attribute_descriptions(&vertex_input_attribute_descriptions)
        .vertex_binding_descriptions(&vertex_input_binding_descriptions);

    let vertex_input_assembly_state_info = vk::PipelineInputAssemblyStateCreateInfo {
        topology: vk::PrimitiveTopology::TRIANGLE_LIST,
        ..Default::default()
    };

    let rasterization_info = vk::PipelineRasterizationStateCreateInfo {
        front_face: vk::FrontFace::COUNTER_CLOCKWISE,
        line_width: 1.0,
        polygon_mode: vk::PolygonMode::FILL,
        ..Default::default()
    };

    let multisample_state_info = vk::PipelineMultisampleStateCreateInfo {
        rasterization_samples: vk::SampleCountFlags::TYPE_1,
        ..Default::default()
    };

    let noop_stencil_state = vk::StencilOpState {
        fail_op: vk::StencilOp::KEEP,
        pass_op: vk::StencilOp::KEEP,
        depth_fail_op: vk::StencilOp::KEEP,
        compare_op: vk::CompareOp::ALWAYS,
        ..Default::default()
    };

    let depth_state_info = vk::PipelineDepthStencilStateCreateInfo {
        depth_test_enable: 1,
        depth_write_enable: 1,
        depth_compare_op: vk::CompareOp::LESS_OR_EQUAL,
        front: noop_stencil_state,
        back: noop_stencil_state,
        max_depth_bounds: 1.0,
        ..Default::default()
    };

    let color_blend_attachment_states = [vk::PipelineColorBlendAttachmentState {
        blend_enable: 0,
        src_color_blend_factor: vk::BlendFactor::SRC_COLOR,
        dst_color_blend_factor: vk::BlendFactor::ONE_MINUS_DST_COLOR,
        color_blend_op: vk::BlendOp::ADD,
        src_alpha_blend_factor: vk::BlendFactor::ZERO,
        dst_alpha_blend_factor: vk::BlendFactor::ZERO,
        alpha_blend_op: vk::BlendOp::ADD,
        color_write_mask: vk::ColorComponentFlags::RGBA,
    }];

    let color_blend_state = vk::PipelineColorBlendStateCreateInfo::default()
        .logic_op(vk::LogicOp::CLEAR)
        .attachments(&color_blend_attachment_states);

    let dynamic_state = [vk::DynamicState::VIEWPORT, vk::DynamicState::SCISSOR];
    let dynamic_state_info =
        vk::PipelineDynamicStateCreateInfo::default().dynamic_states(&dynamic_state);

    let layout_create_info = vk::PipelineLayoutCreateInfo::default();

    let pipeline_layout = unsafe {
        device
            .create_pipeline_layout(&layout_create_info, None)
    }
        .unwrap();

    let dependencies = [vk::SubpassDependency {
        src_subpass: vk::SUBPASS_EXTERNAL,
        src_stage_mask: vk::PipelineStageFlags::COLOR_ATTACHMENT_OUTPUT,
        dst_access_mask: vk::AccessFlags::COLOR_ATTACHMENT_READ
            | vk::AccessFlags::COLOR_ATTACHMENT_WRITE,
        dst_stage_mask: vk::PipelineStageFlags::COLOR_ATTACHMENT_OUTPUT,
        ..Default::default()
    }];

    let renderpass_attachments = [
        vk::AttachmentDescription {
            format: surface_format.format,
            samples: vk::SampleCountFlags::TYPE_1,
            load_op: vk::AttachmentLoadOp::CLEAR,
            store_op: vk::AttachmentStoreOp::STORE,
            final_layout: vk::ImageLayout::PRESENT_SRC_KHR,
            ..Default::default()
        },
        vk::AttachmentDescription {
            format: vk::Format::D16_UNORM,
            samples: vk::SampleCountFlags::TYPE_1,
            load_op: vk::AttachmentLoadOp::CLEAR,
            initial_layout: vk::ImageLayout::DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
            final_layout: vk::ImageLayout::DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
            ..Default::default()
        },
    ];

    let color_attachment_refs = [vk::AttachmentReference {
        attachment: 0,
        layout: vk::ImageLayout::COLOR_ATTACHMENT_OPTIMAL,
    }];
    let depth_attachment_ref = vk::AttachmentReference {
        attachment: 1,
        layout: vk::ImageLayout::DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
    };

    let subpass = vk::SubpassDescription::default()
        .color_attachments(&color_attachment_refs)
        .depth_stencil_attachment(&depth_attachment_ref)
        .pipeline_bind_point(vk::PipelineBindPoint::GRAPHICS);

    let renderpass_create_info = vk::RenderPassCreateInfo::default()
        .attachments(&renderpass_attachments)
        .subpasses(std::slice::from_ref(&subpass))
        .dependencies(&dependencies);

    let renderpass = unsafe {
        device
            .create_render_pass(&renderpass_create_info, None)
    }
        .unwrap();

    let graphic_pipeline_info = vk::GraphicsPipelineCreateInfo::default()
        .stages(&shader_stage_create_infos)
        .vertex_input_state(&vertex_input_state_info)
        .input_assembly_state(&vertex_input_assembly_state_info)
        .viewport_state(&viewport_state_info)
        .rasterization_state(&rasterization_info)
        .multisample_state(&multisample_state_info)
        .depth_stencil_state(&depth_state_info)
        .color_blend_state(&color_blend_state)
        .dynamic_state(&dynamic_state_info)
        .layout(pipeline_layout)
        .render_pass(renderpass);
    

    let graphics_pipelines = unsafe {
        device
            .create_graphics_pipelines(vk::PipelineCache::null(), &[graphic_pipeline_info], None)
    }
        .expect("Unable to create graphics pipeline");
    
    
    window.set_key_polling(true);

    while !window.should_close() {
        glfw.poll_events();
        for (_, event) in glfw::flush_messages(&events) {
            match event {
                glfw::WindowEvent::Key(Key::Escape, _, Action::Press, _) => {
                    window.set_should_close(true)
                }
                _ => {}
            }        
        }

        let (present_index, _) = unsafe { swapchain_loader
            .acquire_next_image(
                swapchain,
                u64::MAX,
                present_complete_semaphore,
                vk::Fence::null(),
            )}
            .unwrap();
        let clear_values = [
            vk::ClearValue {
                color: vk::ClearColorValue {
                    float32: [0.0, 0.0, 0.0, 0.0],
                },
            },
            vk::ClearValue {
                depth_stencil: vk::ClearDepthStencilValue {
                    depth: 1.0,
                    stencil: 0,
                },
            },
        ];

        let render_pass_begin_info = vk::RenderPassBeginInfo::default()
            .render_pass(renderpass)
            .framebuffer(framebuffers[present_index as usize])
            .render_area(surface_resolution.into())
            .clear_values(&clear_values);

        record_submit_commandbuffer(
            &device,
            draw_command_buffer,
            draw_commands_reuse_fence,
            present_queue,
            &[vk::PipelineStageFlags::COLOR_ATTACHMENT_OUTPUT],
            &[present_complete_semaphore],
            &[rendering_complete_semaphore],
            |device, draw_command_buffer| {
                device.cmd_begin_render_pass(
                    draw_command_buffer,
                    &render_pass_begin_info,
                    vk::SubpassContents::INLINE,
                );
                device.cmd_bind_pipeline(
                    draw_command_buffer,
                    vk::PipelineBindPoint::GRAPHICS,
                    graphic_pipeline,
                );
                device.cmd_set_viewport(draw_command_buffer, 0, &viewports);
                device.cmd_set_scissor(draw_command_buffer, 0, &scissors);
                device.cmd_bind_vertex_buffers(
                    draw_command_buffer,
                    0,
                    &[vertex_input_buffer],
                    &[0],
                );
                device.cmd_bind_index_buffer(
                    draw_command_buffer,
                    index_buffer,
                    0,
                    vk::IndexType::UINT32,
                );
                device.cmd_draw_indexed(
                    draw_command_buffer,
                    index_buffer_data.len() as u32,
                    1,
                    0,
                    0,
                    1,
                );
                // Or draw without the index buffer
                // device.cmd_draw(draw_command_buffer, 3, 1, 0, 0);
                device.cmd_end_render_pass(draw_command_buffer);
            },
        );
        let wait_semaphors = [rendering_complete_semaphore];
        let swapchains = [swapchain];
        let image_indices = [present_index];
        let present_info = vk::PresentInfoKHR::default()
            .wait_semaphores(&wait_semaphors) // &rendering_complete_semaphore)
            .swapchains(&swapchains)
            .image_indices(&image_indices);

        unsafe { swapchain_loader
            .queue_present(present_queue, &present_info) }
            .unwrap();
        
    }

    // SyncraOS is the main driver and broker for Syncra. It drives the Vulkan renderer
    // and window/OpenXR context. It manages the lifecycles of various Syncra apps.
    // Each app is a separate C# process that may or may not be sandboxed.
    // You always start off in a local space, which is another C# process
    // with privileged permissions. This is essentially your desktop environment.
}