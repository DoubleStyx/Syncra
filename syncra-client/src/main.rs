use std::ffi::{c_void, CString};
use std::ptr;
use libloading::{Library, Symbol};

#[repr(C)]
pub struct HostfxrHandle(*mut c_void);

type HostfxrInitializeForRuntimeConfig = unsafe extern "C" fn(
    config_path: *const i8,
    reserved: *mut c_void,
    host_context_handle: *mut HostfxrHandle,
) -> i32;

type HostfxrGetDelegate = unsafe extern "C" fn(
    host_context_handle: HostfxrHandle,
    delegate_type: i32,
    delegate: *mut *const c_void,
) -> i32;

type CoreclrCreateDelegate = unsafe extern "C" fn(
    host_context_handle: HostfxrHandle,
    assembly_name: *const i8,
    type_name: *const i8,
    method_name: *const i8,
    delegate: *mut *const c_void,
) -> i32;

fn main() {
    let runtime_config_path = CString::new("path/to/MyLibrary.runtimeconfig.json").unwrap();

    let hostfxr_path = "path/to/hostfxr.dll";
    let lib = unsafe { Library::new(hostfxr_path) }.expect("Failed to load hostfxr.dll");

    let hostfxr_initialize: Symbol<HostfxrInitializeForRuntimeConfig> =
        unsafe { lib.get(b"hostfxr_initialize_for_runtime_config\0") }
            .expect("Failed to locate hostfxr_initialize_for_runtime_config");

    let hostfxr_get_delegate: Symbol<HostfxrGetDelegate> =
        unsafe { lib.get(b"hostfxr_get_runtime_delegate\0") }
            .expect("Failed to locate hostfxr_get_runtime_delegate");

    let coreclr_create_delegate: Symbol<CoreclrCreateDelegate> =
        unsafe { lib.get(b"coreclr_create_delegate\0") }
            .expect("Failed to locate coreclr_create_delegate");

    let mut handle: HostfxrHandle = HostfxrHandle(ptr::null_mut());
    let result = unsafe { hostfxr_initialize(runtime_config_path.as_ptr(), ptr::null_mut(), &mut handle) };

    if result != 0 {
        eprintln!("Failed to initialize the .NET runtime");
        return;
    }

    let mut delegate: *const c_void = ptr::null();
    let assembly_name = CString::new("SyncraEngine").unwrap();
    let type_name = CString::new("SyncraEngine.SyncraEngine").unwrap();
    let method_name = CString::new("Main").unwrap();

    let create_delegate_result = unsafe {
        coreclr_create_delegate(
            handle,
            assembly_name.as_ptr(),
            type_name.as_ptr(),
            method_name.as_ptr(),
            &mut delegate,
        )
    };

    if create_delegate_result != 0 {
        eprintln!("Failed to create delegate for method");
        return;
    }

    let add_fn: unsafe extern "C" fn(i32, i32) -> i32 =
        unsafe { std::mem::transmute(delegate) };

    let result = unsafe { add_fn(2, 3) };
    println!("Result from C#: {}", result);
}
