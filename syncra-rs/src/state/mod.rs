
use std::sync::Mutex;
use lazy_static::lazy_static;
use vr::VRContext;
use crate::vr;
use std::collections::HashMap;

lazy_static! {
    pub static ref VR_CONTEXTS: Mutex<HashMap<u32, VRContext>> = Mutex::new(HashMap::new());
    pub static ref RENDERERS: Mutex<HashMap<u32, Renderer>> = Mutex::new(HashMap::new());
}
