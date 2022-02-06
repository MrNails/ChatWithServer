using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;

namespace SimpleChatServer
{
    public static class Server
    {
        private static readonly string _server = "127.0.0.1";
        private static readonly int _port = 8888;
        private static readonly TcpListener _listener = new TcpListener(IPAddress.Parse(_server), _port);

        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private static bool _isActive = false;
        
        private static List<User> _users = new List<User>();

        private static int count = 0;
        private static int userCount = 0;
        
        public static bool IsActive => _isActive;

        public static Task Start()
        {
            var task = new Task(() =>
            {
                try
                {
                    _listener.Start();
                    
                    _isActive = true;

                    var token = _cancellationTokenSource.Token;

                    token.ThrowIfCancellationRequested();

                    while (true)
                    {
                        var client = _listener.AcceptTcpClient();

                        token.ThrowIfCancellationRequested();

                        var task = new Task(() =>
                        {
                            var server = new ServerClientHandler(client);
                            server.Process();
                            
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
                    _listener.Stop();
                    _isActive = false;
                }
            }, TaskCreationOptions.LongRunning);

            task.Start();

            return task;
        }

        public static void RequestStop()
        {
            _cancellationTokenSource.Cancel();
            
            TcpClient client = new TcpClient(_server, _port);
            client.Close();
        }
    }
}