namespace Syncra;

public class Program
{
    /// <summary>
    /// The main bootstrapping point for the application.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var engine = new Engine();
        engine.Run();
    }
}