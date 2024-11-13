namespace Syncra.Helpers;

public static class BitFlagHelper
{
    public static bool HasFlag(this byte flags, byte position) => (flags & (1 << position)) > 0;
    public static bool HasFlag(this ushort flags, byte position) => (flags & (1u << position)) > 0;
    public static bool HasFlag(this uint flags, byte position) => (flags & (1u << position)) > 0;
    public static bool HasFlag(this ulong flags, byte position) => (flags & (1ul << position)) > 0;
    
    public static byte Flag(this byte flags, byte position, bool value = true) => (byte)(flags | ((value ? 1 : 0) << position));
    public static ushort Flag(this ushort flags, byte position, bool value = true) => (ushort)(flags | ((value ? 1u : 0) << position));
    public static uint Flag(this uint flags, byte position, bool value = true) => (flags | ((value ? 1u : 0) << position));
    public static ulong Flag(this ulong flags, byte position, bool value = true) => (flags | ((value ? 1ul : 0) << position));
    
    public static void SetFlag(this ref byte flags, byte position, bool value = true) => flags = (byte)(flags | ((value ? 1 : 0) << position));
    public static void SetFlag(this ref ushort flags, byte position, bool value = true) => flags = (ushort)(flags | ((value ? 1u : 0) << position));
    public static void SetFlag(this ref uint flags, byte position, bool value = true) => flags = (flags | ((value ? 1u : 0) << position));
    public static void SetFlag(this ref ulong flags, byte position, bool value = true) => flags = (flags | ((value ? 1ul : 0) << position));
    
}