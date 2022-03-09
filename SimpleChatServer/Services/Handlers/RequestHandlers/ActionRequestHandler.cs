using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services.Handlers;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer.Services.Handlers;

public class ActionRequestHandler : IRequestHandler
{
    public Type TypeHandler => typeof(Action);
    
    public Task<ActionResponse> HandleAsync(BinaryReader reader, TcpClient client)
    {
        var action = ActionSerializator.Serializator.Deserialize(reader);
                    
        return ActionHandler.PerformAction(action);
    }
}