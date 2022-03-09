using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services.Handlers;
using SimpleChatServer.Services.StaticMappableServices;

namespace SimpleChatServer.Services.Handlers;

public class RegistrationInfoRequestHandler : IRequestHandler
{
    public Type TypeHandler => typeof(UserCerdentials);

    public Task<ActionResponse> HandleAsync(BinaryReader reader, TcpClient client)
    {
        var registrationInfo = UserCerdentialsSerializator.Serializator.Deserialize(reader);

        return UserManager.CreateUser(registrationInfo, client);
    }
}