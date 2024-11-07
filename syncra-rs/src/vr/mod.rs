use openxr::{Entry, Instance, SystemId, ViewConfigurationType, EnvironmentBlendMode};

pub struct VRContext {
    
}

impl VRContext {
    pub fn new() -> Self {
        let entry = Entry::linked();
        let instance = entry.create_instance(
            &openxr::ApplicationInfo {
                application_name: "Syncra",
                application_version: 0,
                engine_name: "SyncraEngine",
                engine_version: 0,
            },
            &[],
            &[],
        ).unwrap();

        let system = instance.system(openxr::FormFactor::HEAD_MOUNTED_DISPLAY).unwrap();

        Self {
            
        }
    }
}