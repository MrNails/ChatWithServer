using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services.Handlers;
using SimpleChatServer.Services.StaticMappableServices;

namespace SimpleChatServer.Services.Handlers;

public class MessageRequestHandler : IRequestHandler
{
    public Type TypeHandler => typeof(Message);

    public Task<ActionResponse> HandleAsync(BinaryReader reader, TcpClient client)
    {
        var message = MessageSerializator.Serializator.Deserialize(reader);

        Console.WriteLine(
            $"[{message.InChat}] [{message.SendDate.ToString(CultureInfo.InvariantCulture)}] {message.FromUser}: {message.Content}");

        return ChatManager.SendMessageAsync(message);
    }
}