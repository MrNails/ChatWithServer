using System;
using System.IO;

namespace SimpleChatServer.Core.Services.Handlers;

public interface IResponseHandler
{
    Type TypeHandler { get; }
    
    void HandleResponse(BinaryReader reader);
}