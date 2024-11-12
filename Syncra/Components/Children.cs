namespace Syncra.Components;

public struct Children
{
    public List<Node> Value;

    public Children()
    {
        Value = new List<Node>();
    }

    public Children(List<Node> value)
    {
        Value = value;
    }
}