namespace Syncra.DataTypes;

public struct Sync<T> where T : struct
{
    public bool Update;
    public T Value;
}
