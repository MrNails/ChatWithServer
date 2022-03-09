using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services.Handlers;
using SimpleChatServer.DAL;
using SimpleChatServer.Models;
using SimpleChatServer.Services.Exceptions;
using SimpleChatServer.Services.Handlers;

namespace SimpleChatServer
{
    internal static class Server
    {
        private static readonly string s_server;
        private static readonly int s_port;
        private static readonly TcpListener s_listener;

        private static readonly CancellationTokenSource s_cancellationTokenSource;

        private static readonly ServerData s_serverData;
        
        private static bool s_isActive;

        static Server()
        {
            s_server = "127.0.0.1";
            s_port = 8888;
            
            s_listener = new TcpListener(IPAddress.Parse(s_server), s_port);
            s_cancellationTokenSource = new CancellationTokenSource();

            s_isActive = false;

            s_serverData = new ServerData();

            LoadRequestHandlers();
        }

        public static bool IsActive => s_isActive;

        public static ServerData ServerData => s_serverData;

        public static Task Start()
        {
            MasterDataContext mdContext = new MasterDataContext("Data Source=DESKTOP-U6JULRK;Initial Catalog=MasterData;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadOnly;MultiSubnetFailover=False");
            // mdContext.Database.EnsureDeleted();
            // mdContext.Database.EnsureCreated();
            
            // mdContext.Users.Add(new DAL.Models.User() { Name = "Test"});
            mdContext.SaveChanges();
            
            var task = new Task(() =>
            {
                try
                {
                    s_listener.Start();
                    
                    s_isActive = true;
                    
                    Serilog.Log.Logger.Information($"Server started on address: {s_server}:{s_port}");

                    var token = s_cancellationTokenSource.Token;

                    token.ThrowIfCancellationRequested();

                    while (true)
                    {
                        var client = s_listener.AcceptTcpClient();

                        token.ThrowIfCancellationRequested();

                        var task = new Task(() =>
                        {
                            var server = new ServerClientHandler(client);
                            server.Process();
                            
                            Serilog.Log.Logger.Information($"Server started on address: {s_server}:{s_port}");
                            
                        }, TaskCreationOptions.LongRunning);

                        task.Start();
                    }
                }
                catch (OperationCanceledException e)
                {
                    // Serilog.Log.Logger.Error(e);
                    Console.WriteLine("Server stopped.");
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Serilog.Log.Logger.Error(e);
                    throw;
                }
                finally
                {
                    s_listener.Stop();
                    s_isActive = false;
                }
            }, TaskCreationOptions.LongRunning);

            task.Start();

            return task;
        }

        public static void RequestStop()
        {
            s_cancellationTokenSource.Cancel();
            
            TcpClient client = new TcpClient(s_server, s_port);
            client.Close();
        }

        private static void LoadRequestHandlers()
        {
            var assembly = typeof(Server).Assembly;
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsAssignableTo(typeof(IRequestHandler)))
                .ToList();

            if (handlerTypes.Count == 0)
                throw new ServerConfiguringException("Cannot configure request handlers. There are no request handlers.");

            foreach (var handlerType in handlerTypes)
            {
                var handler = (IRequestHandler)Activator.CreateInstance(handlerType);
                
                s_serverData.Handlers.Add(handler.TypeHandler.FullName, handler);
            }
        }
    }
}