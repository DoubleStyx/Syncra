using System.Runtime.InteropServices;

namespace Syncra.Handles;

public class XRContext
{
#if WINDOWS
    [DllImport("../../../../target/debug/syncra_rs.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
    [DllImport("../../../../target/debug/libsyncra_rs.so", CallingConvention = CallingConvention.Cdecl)]
#elif OSX
    [DllImport("../../../../target/debug/libsyncra_rs.dylib", CallingConvention = CallingConvention.Cdecl)]
#endif
    private static extern uint create_xr_context();
    public uint Handle { get; set; }

    public XRContext()
    {
        this.Handle = create_xr_context();
    }
}