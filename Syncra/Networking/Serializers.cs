using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Syncra.DataTypes;

namespace Syncra.Networking;

public static class Serializers
{
    //TODO: a lot of this can probably be optimized with unsafe operations, but i don't feel like doing that right now
    public static void Write(this Stream stream, Quaternion value)
    {
        stream.Write(value.X);
        stream.Write(value.Y);
        stream.Write(value.Z);
        stream.Write(value.W);
    }
    public static void Write(this Stream stream, Vector4 value)
    {
        stream.Write(value.X);
        stream.Write(value.Y);
        stream.Write(value.Z);
        stream.Write(value.W);
    }
    public static void Write(this Stream stream, Vector3 value)
    {
        stream.Write(value.X);
        stream.Write(value.Y);
        stream.Write(value.Z);
    }
    public static void Write(this Stream stream, Vector2 value)
    {
        stream.Write(value.X);
        stream.Write(value.Y);
    }
    public static void Write(this Stream stream, float value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, bool value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, int value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, uint value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, long value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, ulong value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, short value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, ushort value) => stream.Write(BitConverter.GetBytes(value));
    public static void Write(this Stream stream, string value)
    {
        var encoded = Encoding.UTF8.GetBytes(value);
        stream.Write(encoded.Length);
        stream.Write(encoded);
    }
    
    public delegate void WriteAction<T>(Stream stream, T value);
    
    public static void WriteArray<T>(this Stream stream, ICollection<T> collection, WriteAction<T> writer) where T : struct
    {
        stream.Write(collection.Count);
        foreach (var item in collection) writer(stream, item);
    }

    public static void Read(this Stream stream, out Quaternion value)
    {
        stream.Read(out value.X);
        stream.Read(out value.Y);
        stream.Read(out value.Z);
        stream.Read(out value.W);
    }
    public static void Read(this Stream stream, out Vector4 value)
    {
        stream.Read(out value.X);
        stream.Read(out value.Y);
        stream.Read(out value.Z);
        stream.Read(out value.W);
    }
    public static void Read(this Stream stream, out Vector3 value)
    {
        stream.Read(out value.X);
        stream.Read(out value.Y);
        stream.Read(out value.Z);
    }
    public static void Read(this Stream stream, out Vector2 value)
    {
        stream.Read(out value.X);
        stream.Read(out value.Y);
    }
    public static void Read(this Stream stream, out float value)
    {
        var buffer = new byte[sizeof(float)];
        stream.Read(buffer);
        value = BitConverter.ToSingle(buffer);
    }
    public static void Read(this Stream stream, out bool value) => value = stream.ReadByte() > 0;
    public static void Read(this Stream stream, out int value)
    {
        var buffer = new byte[sizeof(int)];
        stream.Read(buffer);
        value = BitConverter.ToInt32(buffer);
    }
    public static void Read(this Stream stream, out uint value)
    {
        var buffer = new byte[sizeof(uint)];
        stream.Read(buffer);
        value = BitConverter.ToUInt32(buffer);
    }
    public static void Read(this Stream stream, out long value)
    {
        var buffer = new byte[sizeof(long)];
        stream.Read(buffer);
        value = BitConverter.ToInt64(buffer);
    }
    public static void Read(this Stream stream, out ulong value)
    {
        var buffer = new byte[sizeof(ulong)];
        stream.Read(buffer);
        value = BitConverter.ToUInt64(buffer);
    }
    public static void Read(this Stream stream, out short value)
    {
        var buffer = new byte[sizeof(short)];
        stream.Read(buffer);
        value = BitConverter.ToInt16(buffer);
    }
    public static void Read(this Stream stream, out ushort value)
    {
        var buffer = new byte[sizeof(ushort)];
        stream.Read(buffer);
        value = BitConverter.ToUInt16(buffer);
    }
    public static void Read(this Stream stream, out string value)
    {
        stream.Read(out int length);
        var buffer = new byte[length];
        stream.Read(buffer);
        value = Encoding.UTF8.GetString(buffer);
    }

    public delegate void ReadAction<T>(Stream stream, out T value);
    
    public static void ReadArray<T>(this Stream stream, ICollection<T> collection, ReadAction<T> reader) where T : struct
    {
        collection.Clear();
        stream.Read(out int length);
        for (var i = 0; i < length; i++)
        {
            reader(stream, out var value);
            collection.Add(value);
        }
    }
}
