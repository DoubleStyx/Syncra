using System.Net;

namespace Syncra.SyncraEngine;

public class Program
{
    public static void Main(string[] args)
    {
        /*
         To launch a new world, the broker process will just start up different SyncraEngine instances by passing this launch argument into them:

        -ip <ip address> -port <port> -session <session guid>
         */

        // launch a new world based on arguments
        IPAddress ip = new IPAddress(new byte[] { 192, 168, 1, 1 });
        int port = 65525;
        Guid sessionGuid = new Guid();
        Guid worldGuid = new Guid();
        World world = new World(ip, port, sessionGuid, worldGuid);
        Thread.Sleep(Timeout.Infinite); // no need for main thread; everything is event-driven
    }
}