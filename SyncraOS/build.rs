use std::{env, fs, path::PathBuf};
use bindgen;
use git2::{FetchOptions, AutotagOption, build::RepoBuilder, build::CheckoutBuilder, Error as GitError};

fn clone_and_checkout(repo_url: &str, repo_dir_path: &PathBuf, version: &str) -> Result<(), GitError> {
    if repo_dir_path.exists() {
        println!("Repository already exists at: {}", repo_dir_path.display());
        return Ok(());
    }

    println!("Cloning repository {} into {}...", repo_url, repo_dir_path.display());

    let mut fetch_opts = FetchOptions::new();
    fetch_opts.download_tags(AutotagOption::All);

    let repo = RepoBuilder::new()
        .fetch_options(fetch_opts)
        .clone(repo_url, repo_dir_path)?;

    println!("Repository cloned successfully. Checking out version: {}", version);

    let object = repo.revparse_single(version)?;
    repo.checkout_tree(&object, Some(&mut CheckoutBuilder::new()))?;
    repo.set_head_detached(object.id())?;

    println!("Checked out version: {}", version);
    Ok(())
}

fn main() {
    println!("cargo:rustc-link-lib=dylib=glfw");
    println!("cargo:rustc-link-lib=vulkan");
    println!("cargo:rustc-link-lib=cglm");
    println!("cargo:rustc-link-lib=openxr_loader");
    println!("cargo:rerun-if-env-changed=VULKAN_INCLUDE_PATH");
    println!("cargo:rerun-if-env-changed=GLFW_INCLUDE_PATH");
    println!("cargo:rerun-if-env-changed=CGLM_INCLUDE_PATH");
    println!("cargo:rerun-if-env-changed=OPENXR_INCLUDE_PATH");

    println!("cargo:rerun-if-changed=build.rs");

    let out_path = PathBuf::from(env::var("OUT_DIR").unwrap());

    let libraries = vec![
        (
            "vulkan",
            "https://github.com/KhronosGroup/Vulkan-Headers.git",
            "v1.3.302",
            "include/vulkan/vulkan.h",
            "VULKAN_INCLUDE_PATH",
            vec!["Vk.*"],
            vec!["VK_.*"],
        ),
        (
            "glfw",
            "https://github.com/glfw/glfw.git",
            "3.4",
            "include/GLFW/glfw3.h",
            "GLFW_INCLUDE_PATH",
            vec!["glfw.*"],
            vec!["GLFW.*"],
        ),
        (
            "cglm",
            "https://github.com/recp/cglm.git",
            "v0.9.4",
            "include/cglm/cglm.h",
            "CGLM_INCLUDE_PATH",
            vec!["glm_.*"],
            vec![".*cglm.*"],
        ),
        (
            "openxr",
            "https://github.com/KhronosGroup/OpenXR-SDK.git",
            "release-1.1.43",
            "include/openxr/openxr.h",
            "OPENXR_INCLUDE_PATH",
            vec!["xr.*"],
            vec!["Xr.*"],
        ),
    ];

    for (lib_name, repo_url, version, header_path, env_var, type_allowlist, var_allowlist) in libraries {
        let repo_dir_path = out_path.join("repos").join(format!("{}-{}", lib_name, version));

        if let Err(e) = clone_and_checkout(repo_url, &repo_dir_path, version) {
            panic!("Failed to clone or checkout repository {}: {}", repo_url, e);
        }

        println!("cargo:rerun-if-changed={}", repo_dir_path.display());

        let include_path = env::var(env_var)
            .unwrap_or_else(|_| repo_dir_path.join("include").to_string_lossy().to_string());
        let header = repo_dir_path.join(header_path);

        println!("Using repo directory: {}", repo_dir_path.display());
        println!("Using header path: {}", header.display());
        println!("Using include path: {}", include_path);

        fs::create_dir_all(out_path.join("bindings"))
            .unwrap_or_else(|e| panic!("Failed to create bindings directory: {}", e));
        let bindings_path = out_path.join("bindings").join(format!("{}.rs", lib_name));
        if !bindings_path.exists() {
            println!("Generating bindings for {}...", lib_name);

            let bindings = bindgen::Builder::default()
                .header(header.to_string_lossy())
                .clang_arg(format!("-I{}", include_path))
                .blocklist_item("FP_NAN") // workaround for cglm duplicate includes
                .blocklist_item("FP_INFINITE")
                .blocklist_item("FP_ZERO")
                .blocklist_item("FP_SUBNORMAL")
                .blocklist_item("FP_NORMAL")
                .generate()
                .unwrap_or_else(|e| panic!("Unable to generate bindings for {}: {}", lib_name, e));

            bindings
                .write_to_file(&bindings_path)
                .unwrap_or_else(|e| panic!("Couldn't write bindings for {}: {}", lib_name, e));
        }
    }
}
