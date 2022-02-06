using System;
using Serilog;
using SimpleChatServer.Services;
using SimpleChatServer.Services.StaticMappableServices;

namespace SimpleChatServer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ActionHandler.AddMappableService(typeof(ChatManager));
            ActionHandler.AddMappableService(typeof(UserManager));
            
            var awaitable = Server.Start().ConfigureAwait(false);
            
            string action = string.Empty;
            bool isExit = false;

            while (!isExit)
            {
                Console.WriteLine("Enter type of action: e - exit.");
                action = Console.ReadLine()!;
                
                switch (action.ToLower())
                {
                    case "e":
                        isExit = true;
                        try
                        {
                            Server.RequestStop();

                            while (Server.IsActive)
                            {
                                Console.WriteLine("Attempt to stop server...");
                                System.Threading.Thread.Sleep(500);
                            }
                            
                            //Get exception if it exists
                            awaitable.GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            
                            // Log.Logger.Error(e, "");
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong action.");
                        break;
                }
            }

            Console.WriteLine("Program main end.");
        }

        public static void ConfigureLogger()
        {
            var logConfiguration = new LoggerConfiguration();
            
            Log.Logger = logConfiguration.WriteTo.File(System.IO.Path.Combine(Environment.CurrentDirectory, "log.txt"),
                                                       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}{NewLine}",
                                                       restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
                                                 .CreateLogger();   
        }
    }
}