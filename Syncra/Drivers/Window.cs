using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace Syncra.Drivers;

/// <summary>
/// Windowing drivers using GLFW.
/// </summary>
public class Window
{
    /// <summary>
    /// A handle to the active window.
    /// </summary>
    private static IWindow _window;
    
    /// <summary>
    /// Creates a new window and maintains a handle to it internally. Start the event loop with Run().
    /// </summary>
    public Window()
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "Syncra"
        };
        _window = Silk.NET.Windowing.Window.Create(options);
    }

    /// <summary>
    /// This will consume the main thread in theory.
    /// </summary>
    public void Run()
    {
        _window.Run();
    }
}