using Silk.NET.Input;
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
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
    }

    /// <summary>
    /// This will consume the main thread in theory.
    /// </summary>
    public void Run()
    {
        _window.Run();
    }

    /// <summary>
    /// Called when the window is first initialized.
    /// </summary>
    private static void OnLoad()
    {
        IInputContext input = _window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    /// <summary>
    /// Called whenever the window state updates.
    /// </summary>
    /// <param name="deltaTime"></param>
    private static void OnUpdate(double deltaTime) { }

    /// <summary>
    /// Called when the window needs a new frame.
    /// </summary>
    /// <param name="deltaTime"></param>
    private static void OnRender(double deltaTime) { }

    /// <summary>
    /// Called when a key is pressed.
    /// </summary>
    /// <param name="keyboard"></param>
    /// <param name="key"></param>
    /// <param name="keyCode"></param>
    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}