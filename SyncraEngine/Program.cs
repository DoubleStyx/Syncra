namespace SyncraEngine;

public class Program
{
    public static List<World> Worlds;
    public static List<IDriver> Drivers;

    public static void Main(string[] args)
    {
        // Driver container
        Drivers = new List<IDriver>();
        // World container
        Worlds = new List<World>();
        Worlds.Add(new World());
        
        // TODO: remove bootstrapping logic and load dedicated home
        // bootstrapping logic goes here
        
        Thread.Sleep(int.MaxValue); // better way to do this later
    }
}