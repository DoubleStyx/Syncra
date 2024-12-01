use std::{env, fs, path::PathBuf};

fn main() {
    println!("cargo:rerun-if-changed=external/");

    let out_dir = PathBuf::from(env::var("OUT_DIR").unwrap());
    let bindings_dir = out_dir.join("bindings");

    fs::create_dir_all(&bindings_dir).expect("Failed to create bindings directory");

    let libraries = vec![
        (
            "vulkan",
            "external/Vulkan-Headers-main/include/vulkan/vulkan.h",
            vec!["-Iexternal/Vulkan-Headers-main/include"],
            "vk.*",
            "Vk.*",
            "VK_.*",
        ),
        (
            "glfw",
            "external/glfw-master/include/GLFW/glfw3.h",
            vec!["-Iexternal/glfw-master/include"],
            "glfw.*",
            "GLFW.*",
            "GLFW_.*",
        ),
        (
            "cglm",
            "external/cglm-master/include/cglm/cglm.h",
            vec!["-Iexternal/cglm-master/include"],
            "cglm.*", // Refined to include only cglm-prefixed functions
            "CGLM.*", // Refined to include only cglm-prefixed types
            "CGLM_.*", // Refined to include only cglm-prefixed constants
        ),
        (
            "openxr",
            "external/OpenXR-SDK-main/include/openxr/openxr.h",
            vec!["-Iexternal/OpenXR-SDK-main/include"],
            "xr.*",
            "Xr.*",
            "XR_.*",
        ),
    ];


    for (lib_name, header_path_str, include_paths, function_allowlist, type_allowlist, var_allowlist) in libraries {
        println!("Generating bindings for {}...", lib_name);

        let header_path = PathBuf::from(header_path_str);
        let bindings_path = bindings_dir.join(format!("bindings_{}.rs", lib_name));

        let mut builder = bindgen::Builder::default()
            .header(header_path.to_string_lossy());

        for include in include_paths {
            builder = builder.clang_arg(include);
        }

        builder = builder
            .allowlist_function(function_allowlist)
            .allowlist_type(type_allowlist)
            .allowlist_var(var_allowlist);

        let bindings = builder
            .generate()
            .expect(&format!("Unable to generate bindings for {}", lib_name));

        bindings
            .write_to_file(&bindings_path)
            .expect(&format!("Couldn't write bindings for {}!", lib_name));
    }

    println!("cargo:rustc-env=BINDINGS_DIR={}", bindings_dir.display());
}
