namespace Syncra;

public static class Engine
{
    public static Dictionary<string, Instance> Instances { get; private set; }
    public static string CurrentInstance { get; private set; }

    public static void Start()
    {
        if (Instances.Count > 0)
            return;
        
        JoinInstance(); // do we want a local home/space?
        Thread.Sleep(int.MaxValue);
    }

    public static void ChangeCurrentInstance(string ID = null)
    {
        
    }

    public static void JoinInstance(string ID = null) // this should create new threads
    {
        
    }

    public static void LeaveInstance(string ID = null)
    {
        
    }
}