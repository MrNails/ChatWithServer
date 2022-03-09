using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;

namespace SimpleChatServer.Core.Services.Handlers;

public interface IRequestHandler
{
    Type TypeHandler { get; }
    
    Task<ActionResponse> HandleAsync(BinaryReader reader, TcpClient client);
}