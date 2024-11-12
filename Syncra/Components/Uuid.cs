namespace Syncra.Components;

public struct Uuid
{
    public Guid Value;

    public Uuid()
    {
        Value = Guid.NewGuid();
    }

    public Uuid(Guid value)
    {
        Value = value;
    }
}