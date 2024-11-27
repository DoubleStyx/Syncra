using System;
using System.Net;
using System.Threading;

namespace Syncra.SyncraEngine
{
    public class Program
    {
        private static ManualResetEvent shutdownEvent = new(false);

        public static void Main(string[] args)
        {
            try
            {
                // Parse launch arguments
                var (ip, port, sessionGuid) = ParseArguments(args);

                // Generate a unique world GUID
                Guid worldGuid = Guid.NewGuid();

                // Launch the world
                Console.WriteLine($"Launching world: IP={ip}, Port={port}, Session={sessionGuid}, World={worldGuid}");
                World world = new World(ip, port, sessionGuid, worldGuid);

                // Wait for shutdown signal
                Console.WriteLine("SyncraEngine is running. Press Ctrl+C to exit.");
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("Shutdown signal received.");
                    e.Cancel = true;
                    shutdownEvent.Set();
                };
                shutdownEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static (IPAddress ip, int port, Guid sessionGuid) ParseArguments(string[] args)
        {
            if (args.Length < 6)
            {
                throw new ArgumentException("Expected arguments: -ip <ip> -port <port> -session <session-guid>");
            }

            string ipString = null;
            int port = -1;
            Guid sessionGuid = Guid.Empty;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-ip":
                        ipString = args[++i];
                        break;
                    case "-port":
                        if (!int.TryParse(args[++i], out port) || port < 0 || port > 65535)
                        {
                            throw new ArgumentException("Port must be a valid number between 0 and 65535.");
                        }
                        break;
                    case "-session":
                        if (!Guid.TryParse(args[++i], out sessionGuid))
                        {
                            throw new ArgumentException("Session GUID is invalid.");
                        }
                        break;
                }
            }

            if (ipString == null || port == -1 || sessionGuid == Guid.Empty)
            {
                throw new ArgumentException("Invalid arguments. Ensure -ip, -port, and -session are specified.");
            }

            return (IPAddress.Parse(ipString), port, sessionGuid);
        }
    }
}
