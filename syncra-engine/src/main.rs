use mun_runtime::Runtime;
use std::env;

fn main() {
    let lib_path = env::args().nth(1).expect("Expected path to a Mun library.");

    let builder = Runtime::builder(lib_path);
    let mut runtime = unsafe { builder.finish() }.expect("Failed to spawn Runtime");

    loop {
        let arg: i64 = runtime.invoke("arg", ()).unwrap();
        let result: i64 = runtime.invoke("fibonacci", (arg,)).unwrap();
        println!("fibonacci({}) = {}", arg, result);

        unsafe { runtime.update() };
    }
}
