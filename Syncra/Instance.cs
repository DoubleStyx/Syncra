using System.Numerics;
using Arch.Core;
using Syncra.Nodes;

namespace Syncra;

public class Instance
{
    public World Entities { get; }
    public Dictionary<BigInteger, Node> Nodes { get; }
    
    public Instance()
    {
        Entities = World.Create();
        Nodes = new Dictionary<BigInteger, Node>();
        
        // debug
        var spinnerNode = new SpinnerNode();
        Nodes.Add(spinnerNode.UUID.Value, spinnerNode);
    }
    
    public void Update()
    {
        foreach (var node in Nodes.Values)
        {
            node.Update();
        }
    }
}
