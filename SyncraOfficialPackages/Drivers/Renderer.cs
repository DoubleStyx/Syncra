using SyncraEngine;

namespace SyncraOfficialPackages.Drivers;

public class Renderer(Window window, Xr xr) : IDriver
{
    public Window Window = window;
    public Xr Xr = xr;
}