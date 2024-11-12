using System.Numerics;

namespace Syncra;

public class Engine
{
    public Dictionary<BigInteger, Instance> Instances { get; }
    public BigInteger CurrentInstance { get; private set; }

    public Engine()
    {
        Instances = new Dictionary<BigInteger, Instance>();
        CurrentInstance = 0;
    }

    public void Start()
    {
        while (true)
        {
            if (Instances.Count > 0)
                return;
        
            JoinInstance(); // do we want a local home/space?
            Thread.Sleep(int.MaxValue);
        }
    }

    public void ChangeCurrentInstance(string ID = null)
    {
        
    }

    public void JoinInstance(string ID = null) // this should create new threads
    {
        
    }

    public void LeaveInstance(string ID = null)
    {
        
    }
}