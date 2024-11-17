use std::ptr;
use x11::xlib;

pub fn main() {
    unsafe {
        // Open a connection to the X server
        let display = xlib::XOpenDisplay(ptr::null());
        if display.is_null() {
            panic!("Unable to open X display");
        }

        // Get the default screen
        let screen = xlib::XDefaultScreen(display);

        // Create a simple window
        let window = xlib::XCreateSimpleWindow(
            display,
            xlib::XRootWindow(display, screen),
            0, 0,  // x, y position
            800, 600,  // width, height
            1,  // border width
            xlib::XBlackPixel(display, screen),
            xlib::XWhitePixel(display, screen),
        );

        // Set the window title
        let window_name = std::ffi::CString::new("My X11 Window").unwrap();
        xlib::XStoreName(display, window, window_name.as_ptr());

        // Select input events
        xlib::XSelectInput(display, window, xlib::ExposureMask | xlib::KeyPressMask);

        // Map (show) the window
        xlib::XMapWindow(display, window);

        // Event loop
        let mut event: xlib::XEvent = std::mem::zeroed();
        loop {
            xlib::XNextEvent(display, &mut event);
            match event.get_type() {
                xlib::Expose => {
                    // Handle expose event (redraw window)
                }
                xlib::KeyPress => {
                    // Handle key press event
                    break;
                }
                _ => {}
            }
        }

        // Close the display connection
        xlib::XCloseDisplay(display);
    }}