using Syncra.Nodes;

namespace Syncra.Components;

public struct Parent
{
    public Node Value;

    public Parent()
    {
        Value = null;
    }

    public Parent(Node value)
    {
        Value = value;
    }
}