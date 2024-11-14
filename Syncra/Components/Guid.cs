namespace Syncra.Components;

public struct Guid
{
    public System.Guid value;

    public Guid()
    {
        value = System.Guid.NewGuid();
    }

    public Guid(System.Guid value)
    {
        this.value = value;
    }
}