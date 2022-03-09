using System;
using System.Runtime.Serialization;

namespace SimpleChatServer.Services.Exceptions;

[Serializable]
public class ServerConfiguringException : Exception
{
    public ServerConfiguringException(string message) : base(message) {}

    public ServerConfiguringException(string message, Exception inner) : base(message, inner) { }
}