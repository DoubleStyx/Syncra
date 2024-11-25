use std::env;
use std::fs;
use std::path::Path;

fn main() {
    let custom_build_dir = "../build/";

    let current_dir = env::current_dir().expect("Failed to get current directory");

    let build_dir = Path::new(&current_dir).join(custom_build_dir);

    if !build_dir.exists() {
        fs::create_dir_all(&build_dir).expect("Failed to create build directory");
    }

    println!("cargo:rustc-env=OUT_DIR={}", build_dir.to_string_lossy());
    println!("cargo:rerun-if-changed=build.rs");
}
