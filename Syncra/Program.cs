using NLog;

namespace Syncra;

public class Program
{
    public static Logger Logger = LogManager.GetCurrentClassLogger();
    static void Main(string[] args)
    {
        Engine.Start();
    }
}