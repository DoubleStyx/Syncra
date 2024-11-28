fn main() {
    // handle input, XR, windowing, audio, and the renderer. Use the main thread to govern all
    // the syncronization logic. Use a job pattern here to manage the different world processes.
    // Each world will also be driven around jobs being dispatched from the main thread.
    // The renderer itself should essentially be abstracted into a high-level framework similar
    // to SRP.
    // For headlesses, just use flags to skip over window initialization and open a console
    // instead.
}
