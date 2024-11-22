namespace SyncraEngine;

public class Renderer
{
    public Window Window;
    public XR Xr;
    public Renderer(Window window, XR xr)
    {
        Window = window;
        Xr = xr;
    }
}