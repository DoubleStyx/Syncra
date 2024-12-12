use bevy::app::App;

fn main() {
    // set up simulated engine context

    // launch runtime as a separate process

    // use IPC between the two

    App::new()
        .add_systems(Startup, add_people)
        .add_systems(Update, (hello_world, (update_people, greet_people).chain()))
        .run();
}
