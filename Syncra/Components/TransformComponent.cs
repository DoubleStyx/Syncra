using System.Numerics;
using Syncra.Helpers;
using Syncra.Networking;

namespace Syncra.Components;

public struct TransformComponent : INetworkedComponent
{
    public Vector3 Position
    {
        get => _pos;
        set
        {
            if (_pos == value) return;
            _pos = value;
            _changedFlags.SetFlag(0);
        }
    }
    public Quaternion Rotation
    {
        get => _rot;
        set
        {
            if (_rot == value) return;
            _rot = value;
            _changedFlags.SetFlag(1);
        }
    }
    public Vector3 Scale
    {
        get => _scale;
        set
        {
            if (_scale == value) return;
            _scale = value;
            _changedFlags.SetFlag(2);
        }
    }

    private Vector3 _pos;
    private Quaternion _rot;
    private Vector3 _scale;
    private byte _changedFlags;

    public void WriteAll(Stream stream)
    {
        stream.Write(_pos);
        stream.Write(_rot);
        stream.Write(_scale);
    }
    public void ReadAll(Stream stream)
    {
        stream.Read(out _pos);
        stream.Read(out _rot);
        stream.Read(out _scale);
    }
    public bool WriteUpdates(Stream stream)
    {
        if (_changedFlags == 0) return false;
        stream.WriteByte(_changedFlags);
        if (_changedFlags.HasFlag(0)) stream.Write(_pos);
        if (_changedFlags.HasFlag(1)) stream.Write(_rot);
        if (_changedFlags.HasFlag(2)) stream.Write(_scale);
        _changedFlags = 0;
        return true;
    }
    public void ReadUpdates(Stream stream)
    {
        var changeFlags = (byte)stream.ReadByte();
        _changedFlags = 0;
        if (changeFlags == 0) return;
        if (changeFlags.HasFlag(0)) stream.Read(out _pos);
        if (changeFlags.HasFlag(1)) stream.Read(out _rot);
        if (changeFlags.HasFlag(2)) stream.Read(out _scale);
    }
}