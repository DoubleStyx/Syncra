namespace Syncra.Components;

public struct Guid
{
    public System.Guid Value;

    public Guid()
    {
        Value = System.Guid.NewGuid();
    }

    public Guid(System.Guid value)
    {
        Value = value;
    }
}