use openxr as xr;
use std::{io::Cursor, sync::{
    atomic::{AtomicBool, Ordering},
    Arc,
}, thread, time::Duration};
use std::sync::Mutex;
use ash::{
    util::read_spv,
    vk::{self, Handle},
};
use lazy_static::lazy_static;

lazy_static! {
    pub static ref XR_THREAD: Arc<Mutex<Option<thread::JoinHandle<()>>>> = Arc::new(Mutex::new(None));
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_create_xr_context() {
        let atomic_bool = Arc::new(AtomicBool::new(false));

        run(atomic_bool);
    }
}


#[no_mangle]
pub extern "C" fn create_xr_context() -> u32 {
    let running = Arc::new(AtomicBool::new(true));
    let r = running.clone();

    let handle = thread::spawn(move || {
        run(r);
    });

    let mut xr_thread = XR_THREAD.lock().unwrap();
    *xr_thread = Some(handle);

    0
}

pub fn run(running: Arc<AtomicBool>) {
    let running = Arc::new(AtomicBool::new(true));
    let r = running.clone();
    ctrlc::set_handler(move || {
        r.store(false, Ordering::Relaxed);
    })
        .expect("setting Ctrl-C handler");

    let entry = unsafe {
        xr::Entry::load()
            .expect("Couldn't find the OpenXR loader. Please ensure an OpenXR runtime is installed and active.")
    };

    #[cfg(target_os = "android")]
    entry.initialize_android_loader().unwrap();

    let available_extensions = entry.enumerate_extensions().unwrap();

    assert!(available_extensions.khr_vulkan_enable2);

    let mut enabled_extensions = xr::ExtensionSet::default();
    enabled_extensions.khr_vulkan_enable2 = true;
    #[cfg(target_os = "android")]
    {
        enabled_extensions.khr_android_create_instance = true;
    }
    let xr_instance = entry
        .create_instance(
            &xr::ApplicationInfo {
                application_name: "openxrs example",
                application_version: 0,
                engine_name: "openxrs example",
                engine_version: 0,
                api_version: xr::Version::new(1, 0, 0),
            },
            &enabled_extensions,
            &[],
        )
        .unwrap();
    let instance_props = xr_instance.properties().unwrap();
    println!(
        "loaded OpenXR runtime: {} {}",
        instance_props.runtime_name, instance_props.runtime_version
    );

    let system = xr_instance
        .system(xr::FormFactor::HEAD_MOUNTED_DISPLAY)
        .unwrap();

    let environment_blend_mode = xr_instance
        .enumerate_environment_blend_modes(system, VIEW_TYPE)
        .unwrap()[0];

    let vk_target_version = vk::make_api_version(0, 1, 1, 0); // Vulkan 1.1 guarantees multiview support
    let vk_target_version_xr = xr::Version::new(1, 1, 0);

    let reqs = xr_instance
        .graphics_requirements::<xr::Vulkan>(system)
        .unwrap();

    if vk_target_version_xr < reqs.min_api_version_supported
        || vk_target_version_xr.major() > reqs.max_api_version_supported.major()
    {
        panic!(
            "OpenXR runtime requires Vulkan version > {}, < {}.0.0",
            reqs.min_api_version_supported,
            reqs.max_api_version_supported.major() + 1
        );
    }

    #[allow(clippy::missing_transmute_annotations)]
    unsafe {
        let vk_entry = ash::Entry::load().unwrap();

        let vk_app_info = vk::ApplicationInfo::default()
            .application_version(0)
            .engine_version(0)
            .api_version(vk_target_version);

        let vk_instance = {
            let vk_instance = xr_instance
                .create_vulkan_instance(
                    system,
                    std::mem::transmute(vk_entry.static_fn().get_instance_proc_addr),
                    &vk::InstanceCreateInfo::default().application_info(&vk_app_info) as *const _
                        as *const _,
                )
                .expect("XR error creating Vulkan instance")
                .map_err(vk::Result::from_raw)
                .expect("Vulkan error creating Vulkan instance");
            ash::Instance::load(
                vk_entry.static_fn(),
                vk::Instance::from_raw(vk_instance as _),
            )
        };

        let vk_physical_device = vk::PhysicalDevice::from_raw(
            xr_instance
                .vulkan_graphics_device(system, vk_instance.handle().as_raw() as _)
                .unwrap() as _,
        );

        let vk_device_properties = vk_instance.get_physical_device_properties(vk_physical_device);
        if vk_device_properties.api_version < vk_target_version {
            vk_instance.destroy_instance(None);
            panic!("Vulkan phyiscal device doesn't support version 1.1");
        }

        let queue_family_index = vk_instance
            .get_physical_device_queue_family_properties(vk_physical_device)
            .into_iter()
            .enumerate()
            .find_map(|(queue_family_index, info)| {
                if info.queue_flags.contains(vk::QueueFlags::GRAPHICS) {
                    Some(queue_family_index as u32)
                } else {
                    None
                }
            })
            .expect("Vulkan device has no graphics queue");

        let vk_device = {
            let vk_device = xr_instance
                .create_vulkan_device(
                    system,
                    std::mem::transmute(vk_entry.static_fn().get_instance_proc_addr),
                    vk_physical_device.as_raw() as _,
                    &vk::DeviceCreateInfo::default()
                        .queue_create_infos(&[vk::DeviceQueueCreateInfo::default()
                            .queue_family_index(queue_family_index)
                            .queue_priorities(&[1.0])])
                        .push_next(&mut vk::PhysicalDeviceMultiviewFeatures {
                            multiview: vk::TRUE,
                            ..Default::default()
                        }) as *const _ as *const _,
                )
                .expect("XR error creating Vulkan device")
                .map_err(vk::Result::from_raw)
                .expect("Vulkan error creating Vulkan device");

            ash::Device::load(vk_instance.fp_v1_0(), vk::Device::from_raw(vk_device as _))
        };

        let queue = vk_device.get_device_queue(queue_family_index, 0);

        let view_mask = !(!0 << VIEW_COUNT);
        let render_pass = vk_device
            .create_render_pass(
                &vk::RenderPassCreateInfo::default()
                    .attachments(&[vk::AttachmentDescription {
                        format: COLOR_FORMAT,
                        samples: vk::SampleCountFlags::TYPE_1,
                        load_op: vk::AttachmentLoadOp::CLEAR,
                        store_op: vk::AttachmentStoreOp::STORE,
                        initial_layout: vk::ImageLayout::UNDEFINED,
                        final_layout: vk::ImageLayout::COLOR_ATTACHMENT_OPTIMAL,
                        ..Default::default()
                    }])
                    .subpasses(&[vk::SubpassDescription::default()
                        .color_attachments(&[vk::AttachmentReference {
                            attachment: 0,
                            layout: vk::ImageLayout::COLOR_ATTACHMENT_OPTIMAL,
                        }])
                        .pipeline_bind_point(vk::PipelineBindPoint::GRAPHICS)])
                    .dependencies(&[vk::SubpassDependency {
                        src_subpass: vk::SUBPASS_EXTERNAL,
                        dst_subpass: 0,
                        src_stage_mask: vk::PipelineStageFlags::COLOR_ATTACHMENT_OUTPUT,
                        dst_stage_mask: vk::PipelineStageFlags::COLOR_ATTACHMENT_OUTPUT,
                        dst_access_mask: vk::AccessFlags::COLOR_ATTACHMENT_WRITE,
                        ..Default::default()
                    }])
                    .push_next(
                        &mut vk::RenderPassMultiviewCreateInfo::default()
                            .view_masks(&[view_mask])
                            .correlation_masks(&[view_mask]),
                    ),
                None,
            )
            .unwrap();

        let vert = read_spv(&mut Cursor::new(&include_bytes!("fullscreen.vert.spv")[..])).unwrap();
        let frag = read_spv(&mut Cursor::new(
            &include_bytes!("debug_pattern.frag.spv")[..],
        ))
            .unwrap();
        let vert = vk_device
            .create_shader_module(&vk::ShaderModuleCreateInfo::default().code(&vert), None)
            .unwrap();
        let frag = vk_device
            .create_shader_module(&vk::ShaderModuleCreateInfo::default().code(&frag), None)
            .unwrap();

        let pipeline_layout = vk_device
            .create_pipeline_layout(
                &vk::PipelineLayoutCreateInfo::default().set_layouts(&[]),
                None,
            )
            .unwrap();
        let noop_stencil_state = vk::StencilOpState {
            fail_op: vk::StencilOp::KEEP,
            pass_op: vk::StencilOp::KEEP,
            depth_fail_op: vk::StencilOp::KEEP,
            compare_op: vk::CompareOp::ALWAYS,
            compare_mask: 0,
            write_mask: 0,
            reference: 0,
        };
        let pipeline = vk_device
            .create_graphics_pipelines(
                vk::PipelineCache::null(),
                &[vk::GraphicsPipelineCreateInfo::default()
                    .stages(&[
                        vk::PipelineShaderStageCreateInfo {
                            stage: vk::ShaderStageFlags::VERTEX,
                            module: vert,
                            p_name: b"main\0".as_ptr() as _,
                            ..Default::default()
                        },
                        vk::PipelineShaderStageCreateInfo {
                            stage: vk::ShaderStageFlags::FRAGMENT,
                            module: frag,
                            p_name: b"main\0".as_ptr() as _,
                            ..Default::default()
                        },
                    ])
                    .vertex_input_state(&vk::PipelineVertexInputStateCreateInfo::default())
                    .input_assembly_state(
                        &vk::PipelineInputAssemblyStateCreateInfo::default()
                            .topology(vk::PrimitiveTopology::TRIANGLE_LIST),
                    )
                    .viewport_state(
                        &vk::PipelineViewportStateCreateInfo::default()
                            .scissor_count(1)
                            .viewport_count(1),
                    )
                    .rasterization_state(
                        &vk::PipelineRasterizationStateCreateInfo::default()
                            .cull_mode(vk::CullModeFlags::NONE)
                            .polygon_mode(vk::PolygonMode::FILL)
                            .line_width(1.0),
                    )
                    .multisample_state(
                        &vk::PipelineMultisampleStateCreateInfo::default()
                            .rasterization_samples(vk::SampleCountFlags::TYPE_1),
                    )
                    .depth_stencil_state(
                        &vk::PipelineDepthStencilStateCreateInfo::default()
                            .depth_test_enable(false)
                            .depth_write_enable(false)
                            .front(noop_stencil_state)
                            .back(noop_stencil_state),
                    )
                    .color_blend_state(
                        &vk::PipelineColorBlendStateCreateInfo::default().attachments(&[
                            vk::PipelineColorBlendAttachmentState {
                                blend_enable: vk::TRUE,
                                src_color_blend_factor: vk::BlendFactor::ONE,
                                dst_color_blend_factor: vk::BlendFactor::ZERO,
                                color_blend_op: vk::BlendOp::ADD,
                                color_write_mask: vk::ColorComponentFlags::R
                                    | vk::ColorComponentFlags::G
                                    | vk::ColorComponentFlags::B,
                                ..Default::default()
                            },
                        ]),
                    )
                    .dynamic_state(
                        &vk::PipelineDynamicStateCreateInfo::default().dynamic_states(&[
                            vk::DynamicState::VIEWPORT,
                            vk::DynamicState::SCISSOR,
                        ]),
                    )
                    .layout(pipeline_layout)
                    .render_pass(render_pass)
                    .subpass(0)],
                None,
            )
            .unwrap()[0];

        vk_device.destroy_shader_module(vert, None);
        vk_device.destroy_shader_module(frag, None);

        let (session, mut frame_wait, mut frame_stream) = xr_instance
            .create_session::<xr::Vulkan>(
                system,
                &xr::vulkan::SessionCreateInfo {
                    instance: vk_instance.handle().as_raw() as _,
                    physical_device: vk_physical_device.as_raw() as _,
                    device: vk_device.handle().as_raw() as _,
                    queue_family_index,
                    queue_index: 0,
                },
            )
            .unwrap();

        let action_set = xr_instance
            .create_action_set("input", "input pose information", 0)
            .unwrap();

        let right_action = action_set
            .create_action::<xr::Posef>("right_hand", "Right Hand Controller", &[])
            .unwrap();
        let left_action = action_set
            .create_action::<xr::Posef>("left_hand", "Left Hand Controller", &[])
            .unwrap();

        xr_instance
            .suggest_interaction_profile_bindings(
                xr_instance
                    .string_to_path("/interaction_profiles/khr/simple_controller")
                    .unwrap(),
                &[
                    xr::Binding::new(
                        &right_action,
                        xr_instance
                            .string_to_path("/user/hand/right/input/grip/pose")
                            .unwrap(),
                    ),
                    xr::Binding::new(
                        &left_action,
                        xr_instance
                            .string_to_path("/user/hand/left/input/grip/pose")
                            .unwrap(),
                    ),
                ],
            )
            .unwrap();

        session.attach_action_sets(&[&action_set]).unwrap();

        let right_space = right_action
            .create_space(session.clone(), xr::Path::NULL, xr::Posef::IDENTITY)
            .unwrap();
        let left_space = left_action
            .create_space(session.clone(), xr::Path::NULL, xr::Posef::IDENTITY)
            .unwrap();

        let stage = session
            .create_reference_space(xr::ReferenceSpaceType::STAGE, xr::Posef::IDENTITY)
            .unwrap();

        let cmd_pool = vk_device
            .create_command_pool(
                &vk::CommandPoolCreateInfo::default()
                    .queue_family_index(queue_family_index)
                    .flags(
                        vk::CommandPoolCreateFlags::RESET_COMMAND_BUFFER
                            | vk::CommandPoolCreateFlags::TRANSIENT,
                    ),
                None,
            )
            .unwrap();
        let cmds = vk_device
            .allocate_command_buffers(
                &vk::CommandBufferAllocateInfo::default()
                    .command_pool(cmd_pool)
                    .command_buffer_count(PIPELINE_DEPTH),
            )
            .unwrap();
        let fences = (0..PIPELINE_DEPTH)
            .map(|_| {
                vk_device
                    .create_fence(
                        &vk::FenceCreateInfo::default().flags(vk::FenceCreateFlags::SIGNALED),
                        None,
                    )
                    .unwrap()
            })
            .collect::<Vec<_>>();

        let mut swapchain = None;
        let mut event_storage = xr::EventDataBuffer::new();
        let mut session_running = false;

        let mut frame = 0;
        'main_loop: loop {
            if !running.load(Ordering::Relaxed) {
                println!("requesting exit");
                match session.request_exit() {
                    Ok(()) => {}
                    Err(xr::sys::Result::ERROR_SESSION_NOT_RUNNING) => break,
                    Err(e) => panic!("{}", e),
                }
            }

            while let Some(event) = xr_instance.poll_event(&mut event_storage).unwrap() {
                use xr::Event::*;
                match event {
                    SessionStateChanged(e) => {
                        println!("entered state {:?}", e.state());
                        match e.state() {
                            xr::SessionState::READY => {
                                session.begin(VIEW_TYPE).unwrap();
                                session_running = true;
                            }
                            xr::SessionState::STOPPING => {
                                session.end().unwrap();
                                session_running = false;
                            }
                            xr::SessionState::EXITING | xr::SessionState::LOSS_PENDING => {
                                break 'main_loop;
                            }
                            _ => {}
                        }
                    }
                    InstanceLossPending(_) => {
                        break 'main_loop;
                    }
                    EventsLost(e) => {
                        println!("lost {} events", e.lost_event_count());
                    }
                    _ => {}
                }
            }

            if !session_running {
                std::thread::sleep(Duration::from_millis(100));
                continue;
            }

            let xr_frame_state = frame_wait.wait().unwrap();
            frame_stream.begin().unwrap();

            if !xr_frame_state.should_render {
                frame_stream
                    .end(
                        xr_frame_state.predicted_display_time,
                        environment_blend_mode,
                        &[],
                    )
                    .unwrap();
                continue;
            }

            let swapchain = swapchain.get_or_insert_with(|| {
                let views = xr_instance
                    .enumerate_view_configuration_views(system, VIEW_TYPE)
                    .unwrap();
                assert_eq!(views.len(), VIEW_COUNT as usize);
                assert_eq!(views[0], views[1]);

                let resolution = vk::Extent2D {
                    width: views[0].recommended_image_rect_width,
                    height: views[0].recommended_image_rect_height,
                };
                let handle = session
                    .create_swapchain(&xr::SwapchainCreateInfo {
                        create_flags: xr::SwapchainCreateFlags::EMPTY,
                        usage_flags: xr::SwapchainUsageFlags::COLOR_ATTACHMENT
                            | xr::SwapchainUsageFlags::SAMPLED,
                        format: COLOR_FORMAT.as_raw() as _,
                        sample_count: 1,
                        width: resolution.width,
                        height: resolution.height,
                        face_count: 1,
                        array_size: VIEW_COUNT,
                        mip_count: 1,
                    })
                    .unwrap();

                let images = handle.enumerate_images().unwrap();
                Swapchain {
                    handle,
                    resolution,
                    buffers: images
                        .into_iter()
                        .map(|color_image| {
                            let color_image = vk::Image::from_raw(color_image);
                            let color = vk_device
                                .create_image_view(
                                    &vk::ImageViewCreateInfo::default()
                                        .image(color_image)
                                        .view_type(vk::ImageViewType::TYPE_2D_ARRAY)
                                        .format(COLOR_FORMAT)
                                        .subresource_range(vk::ImageSubresourceRange {
                                            aspect_mask: vk::ImageAspectFlags::COLOR,
                                            base_mip_level: 0,
                                            level_count: 1,
                                            base_array_layer: 0,
                                            layer_count: VIEW_COUNT,
                                        }),
                                    None,
                                )
                                .unwrap();
                            let framebuffer = vk_device
                                .create_framebuffer(
                                    &vk::FramebufferCreateInfo::default()
                                        .render_pass(render_pass)
                                        .width(resolution.width)
                                        .height(resolution.height)
                                        .attachments(&[color])
                                        .layers(1),
                                    None,
                                )
                                .unwrap();
                            Framebuffer { framebuffer, color }
                        })
                        .collect(),
                }
            });

            let image_index = swapchain.handle.acquire_image().unwrap();

            vk_device
                .wait_for_fences(&[fences[frame]], true, u64::MAX)
                .unwrap();
            vk_device.reset_fences(&[fences[frame]]).unwrap();

            let cmd = cmds[frame];
            vk_device
                .begin_command_buffer(
                    cmd,
                    &vk::CommandBufferBeginInfo::default()
                        .flags(vk::CommandBufferUsageFlags::ONE_TIME_SUBMIT),
                )
                .unwrap();
            vk_device.cmd_begin_render_pass(
                cmd,
                &vk::RenderPassBeginInfo::default()
                    .render_pass(render_pass)
                    .framebuffer(swapchain.buffers[image_index as usize].framebuffer)
                    .render_area(vk::Rect2D {
                        offset: vk::Offset2D::default(),
                        extent: swapchain.resolution,
                    })
                    .clear_values(&[vk::ClearValue {
                        color: vk::ClearColorValue {
                            float32: [0.0, 0.0, 0.0, 1.0],
                        },
                    }]),
                vk::SubpassContents::INLINE,
            );

            let viewports = [vk::Viewport {
                x: 0.0,
                y: 0.0,
                width: swapchain.resolution.width as f32,
                height: swapchain.resolution.height as f32,
                min_depth: 0.0,
                max_depth: 1.0,
            }];
            let scissors = [vk::Rect2D {
                offset: vk::Offset2D { x: 0, y: 0 },
                extent: swapchain.resolution,
            }];
            vk_device.cmd_set_viewport(cmd, 0, &viewports);
            vk_device.cmd_set_scissor(cmd, 0, &scissors);

            vk_device.cmd_bind_pipeline(cmd, vk::PipelineBindPoint::GRAPHICS, pipeline);
            vk_device.cmd_draw(cmd, 3, 1, 0, 0);

            vk_device.cmd_end_render_pass(cmd);
            vk_device.end_command_buffer(cmd).unwrap();

            session.sync_actions(&[(&action_set).into()]).unwrap();

            let right_location = right_space
                .locate(&stage, xr_frame_state.predicted_display_time)
                .unwrap();

            let left_location = left_space
                .locate(&stage, xr_frame_state.predicted_display_time)
                .unwrap();

            let mut printed = false;
            if left_action.is_active(&session, xr::Path::NULL).unwrap() {
                print!(
                    "Left Hand: ({:0<12},{:0<12},{:0<12}), ",
                    left_location.pose.position.x,
                    left_location.pose.position.y,
                    left_location.pose.position.z
                );
                printed = true;
            }

            if right_action.is_active(&session, xr::Path::NULL).unwrap() {
                print!(
                    "Right Hand: ({:0<12},{:0<12},{:0<12})",
                    right_location.pose.position.x,
                    right_location.pose.position.y,
                    right_location.pose.position.z
                );
                printed = true;
            }
            if printed {
                println!();
            }

            let (_, views) = session
                .locate_views(VIEW_TYPE, xr_frame_state.predicted_display_time, &stage)
                .unwrap();

            swapchain.handle.wait_image(xr::Duration::INFINITE).unwrap();

            vk_device
                .queue_submit(
                    queue,
                    &[vk::SubmitInfo::default().command_buffers(&[cmd])],
                    fences[frame],
                )
                .unwrap();
            swapchain.handle.release_image().unwrap();

            let rect = xr::Rect2Di {
                offset: xr::Offset2Di { x: 0, y: 0 },
                extent: xr::Extent2Di {
                    width: swapchain.resolution.width as _,
                    height: swapchain.resolution.height as _,
                },
            };
            frame_stream
                .end(
                    xr_frame_state.predicted_display_time,
                    environment_blend_mode,
                    &[
                        &xr::CompositionLayerProjection::new().space(&stage).views(&[
                            xr::CompositionLayerProjectionView::new()
                                .pose(views[0].pose)
                                .fov(views[0].fov)
                                .sub_image(
                                    xr::SwapchainSubImage::new()
                                        .swapchain(&swapchain.handle)
                                        .image_array_index(0)
                                        .image_rect(rect),
                                ),
                            xr::CompositionLayerProjectionView::new()
                                .pose(views[1].pose)
                                .fov(views[1].fov)
                                .sub_image(
                                    xr::SwapchainSubImage::new()
                                        .swapchain(&swapchain.handle)
                                        .image_array_index(1)
                                        .image_rect(rect),
                                ),
                        ]),
                    ],
                )
                .unwrap();
            frame = (frame + 1) % PIPELINE_DEPTH as usize;
        }

        drop((
            session,
            frame_wait,
            frame_stream,
            stage,
            action_set,
            left_space,
            right_space,
            left_action,
            right_action,
        ));

        vk_device.wait_for_fences(&fences, true, !0).unwrap();
        for fence in fences {
            vk_device.destroy_fence(fence, None);
        }

        if let Some(swapchain) = swapchain {
            for buffer in swapchain.buffers {
                vk_device.destroy_framebuffer(buffer.framebuffer, None);
                vk_device.destroy_image_view(buffer.color, None);
            }
        }

        vk_device.destroy_pipeline(pipeline, None);
        vk_device.destroy_pipeline_layout(pipeline_layout, None);
        vk_device.destroy_command_pool(cmd_pool, None);
        vk_device.destroy_render_pass(render_pass, None);
        vk_device.destroy_device(None);
        vk_instance.destroy_instance(None);
    }

    println!("exiting cleanly");
}

pub const COLOR_FORMAT: vk::Format = vk::Format::R8G8B8A8_SRGB;
pub const VIEW_COUNT: u32 = 2;
const VIEW_TYPE: xr::ViewConfigurationType = xr::ViewConfigurationType::PRIMARY_STEREO;

struct Swapchain {
    handle: xr::Swapchain<xr::Vulkan>,
    buffers: Vec<Framebuffer>,
    resolution: vk::Extent2D,
}

struct Framebuffer {
    framebuffer: vk::Framebuffer,
    color: vk::ImageView,
}

const PIPELINE_DEPTH: u32 = 2;