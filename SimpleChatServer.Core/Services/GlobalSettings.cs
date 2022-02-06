using System;
using System.Net.Sockets;

namespace SimpleChatServer.Core.Services
{
    internal static class GlobalSettings
    {
        private static readonly string s_server;
        private static readonly int s_port;
        
        public static readonly TcpClient CurrentClient;

        static GlobalSettings()
        {
            s_server = "127.0.0.1";
            s_port = 8888;
            
            CurrentClient = new TcpClient(s_server, s_port);
        }
    }
}