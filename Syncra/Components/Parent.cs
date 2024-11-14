namespace Syncra.Components;

public struct Parent
{
    public System.Guid value;

    public Parent()
    {
        value = System.Guid.NewGuid();
    }

    public Parent(System.Guid value)
    {
        this.value = value;
    }
}