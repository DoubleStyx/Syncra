use std::{env, fs, path::PathBuf};
use bindgen;
use git2::{Repository, FetchOptions, AutotagOption, build::RepoBuilder};

fn main() {
    println!("cargo:rerun-if-changed=external/");

    let repositories = vec![
        (
            "vulkan",
            "https://github.com/KhronosGroup/Vulkan-Headers.git",
            "external/Vulkan-Headers-main",
            "v1.3.302",
            "include/vulkan/vulkan.h",
            vec!["-Iexternal/Vulkan-Headers-main/include"],
            vec!["vulkan.*"],
            vec!["Vk.*"],
            vec!["VK.*"],
        ),
        (
            "glfw",
            "https://github.com/glfw/glfw.git",
            "external/glfw-master",
            "3.4",
            "include/GLFW/glfw3.h",
            vec!["-Iexternal/glfw-master/include"],
            vec!["glfw.*"],
            vec!["GLFW.*"],
            vec!["GLFW_.*"],
        ),
        (
            "cglm",
            "https://github.com/recp/cglm.git",
            "external/cglm-master",
            "v0.9.4",
            "include/cglm/cglm.h",
            vec!["-Iexternal/cglm-master/include"],
            vec!["glm_.*"],
            vec![".*cglm.*"],
            vec!["CGLM.*"],
        ),
        (
            "openxr",
            "https://github.com/KhronosGroup/OpenXR-SDK.git",
            "external/OpenXR-SDK-main",
            "release-1.1.43",
            "include/openxr/openxr.h",
            vec!["-Iexternal/OpenXR-SDK-main/include"],
            vec!["xr.*"],
            vec!["Xr.*"],
            vec!["XR_.*"],
        ),
    ];
    
    let bindings_dir = PathBuf::from("src/bindings");
    fs::create_dir_all(&bindings_dir).expect("Failed to create bindings directory");

    let external_dir = PathBuf::from("external");
    if !external_dir.exists() {
        fs::create_dir_all(&external_dir).expect("Failed to create external directory");
    }

    for (lib_name, repo_url, repo_dir, version, header_path, include_paths, function_allowlist, type_allowlist, var_allowlist) in repositories {
        let repo_dir_path = PathBuf::from(repo_dir);
        if !repo_dir_path.exists() {
            println!("Cloning repository {} into {}...", repo_url, repo_dir_path.display());
            let mut fetch_opts = FetchOptions::new();
            fetch_opts.download_tags(AutotagOption::All);
            let mut builder = RepoBuilder::new();
            builder.fetch_options(fetch_opts);
            match builder.clone(repo_url, &repo_dir_path) {
                Ok(_) => println!("Successfully cloned repository: {}", lib_name),
                Err(e) => panic!("Failed to clone repository {}: {}", repo_url, e),
            }
        }

        let repo = Repository::open(&repo_dir_path).expect("Failed to open cloned repository");

        // Find the specific version tag
        let obj = repo
            .revparse_single(&format!("refs/tags/{}", version))
            .unwrap_or_else(|e| panic!("Version {} not found in repository {}: {}", version, lib_name, e));

        // Peel the object to a commit
        let commit = obj.peel_to_commit()
            .unwrap_or_else(|e| panic!("Failed to peel object to commit for {}: {}", lib_name, e));

        // Checkout the commit
        repo.checkout_tree(commit.as_object(), None)
            .unwrap_or_else(|e| panic!("Failed to checkout tree for {}: {}", lib_name, e));

        // Set HEAD to the commit
        repo.set_head_detached(commit.id())
            .unwrap_or_else(|e| panic!("Failed to set HEAD for {}: {}", lib_name, e));

        println!("Generating bindings for {}...", lib_name);
        let header_path = repo_dir_path.join(header_path);
        if !header_path.exists() {
            panic!("Header file not found: {}", header_path.display());
        }

        let bindings_path = bindings_dir.join(format!("{}.rs", lib_name));

        let mut builder = bindgen::Builder::default()
            .header(header_path.to_string_lossy());

        for include in include_paths {
            builder = builder.clang_arg(include);
        }

        for pattern in function_allowlist {
            builder = builder.allowlist_function(pattern);
        }

        for pattern in type_allowlist {
            builder = builder.allowlist_type(pattern);
        }

        for pattern in var_allowlist {
            builder = builder.allowlist_var(pattern);
        }

        let bindings = builder
            .generate()
            .expect(&format!("Unable to generate bindings for {}", lib_name));

        bindings
            .write_to_file(&bindings_path)
            .expect(&format!("Couldn't write bindings for {}!", lib_name));
    }

    println!("Bindings successfully generated in src/bindings.");
}