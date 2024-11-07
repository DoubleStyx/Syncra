namespace Syncra;

public class Program
{
    static void Main(string[] args)
    {
        var game = new World();
        game.Initialize();

        while (true)
        {
            game.Update();
            System.Threading.Thread.Sleep(16);
        }

        game.Dispose();
    }
}
