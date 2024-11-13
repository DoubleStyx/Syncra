using NLog;

namespace Syncra;

public class Program
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();
    static void Main(string[] args)
    {
        Engine engine = new Engine();
        engine.Run();
    }
}