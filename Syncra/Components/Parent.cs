namespace Syncra.Components;

public struct Parent
{
    public Guid value;

    public Parent()
    {
        value = new Guid();
    }

    public Parent(Guid value)
    {
        this.value = value;
    }
}