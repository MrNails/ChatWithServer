using System;
using System.Collections.Generic;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.SerializationResolvers.ExceptionResolvers;
using SimpleChatServer.Core.Services.Exceptions;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer.Core.Services
{
    public static class Utilities
    {
        public static readonly Dictionary<string, ISerializator> Serializators = new Dictionary<string, ISerializator>();

        static Utilities()
        {
            Serializators.Add(typeof(ChatNotFoundException).FullName!, ChatNotFoundSerializator.Serializator);
            Serializators.Add(typeof(ChatRestrictionException).FullName!, ChatRestrictionSerializator.Serializator);
            Serializators.Add(typeof(UserExistsException).FullName!, UserExistsSerializator.Serializator);
            Serializators.Add(typeof(Action).FullName!, ActionSerializator.Serializator);
            Serializators.Add(typeof(Chat).FullName!, ChatSerializator.Serializator);
            Serializators.Add(typeof(Message).FullName!, MessageSerializator.Serializator);
            Serializators.Add(typeof(RegistrationInfo).FullName!, RegistrationInfoSerializator.Serializator);
            Serializators.Add(typeof(TransmitObject).FullName!, TransmitObjectSerializator.Serializator);
            Serializators.Add(typeof(User).FullName!, UserSerializator.Serializator);
            Serializators.Add(typeof(Primitive).FullName!, PrimitiveSerializator.Serializator);
        }
    }
}