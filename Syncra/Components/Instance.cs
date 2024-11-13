namespace Syncra.Components;

public struct Instance
{
    public Syncra.Instance Value;

    public Instance()
    {
        Value = null;
    }

    public Instance(Syncra.Instance value)
    {
        Value = value;
    }
}