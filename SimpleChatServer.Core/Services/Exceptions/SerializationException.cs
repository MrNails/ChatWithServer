using System;

namespace SimpleChatServer.Core.Services.Exceptions
{
    public class SerializationException : Exception
    {
        public SerializationException(Type type, string message) : base(message)
        {
            Type = type;
        }
        
        public Type Type { get; }
    }
}