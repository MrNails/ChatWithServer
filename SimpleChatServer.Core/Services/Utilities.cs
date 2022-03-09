using System;
using System.Collections.Generic;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Restrictions;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.SerializationResolvers.ExceptionResolvers;
using SimpleChatServer.Core.Services.Exceptions;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer.Core.Services
{
    public static class Utilities
    {
        public static readonly Dictionary<string, ISerializator> Serializators;
        public static readonly ChatRestriction ChatRestriction;

        static Utilities()
        {
            Serializators = new Dictionary<string, ISerializator>();
            ChatRestriction = new ChatRestriction(50, 5, 128);

            FillSerializators();
        }

        private static void FillSerializators()
        {
            Serializators.Add(typeof(ChatNotFoundException).FullName!, ChatNotFoundSerializator.Serializator);
            Serializators.Add(typeof(ChatRestrictionException).FullName!, ChatRestrictionSerializator.Serializator);
            Serializators.Add(typeof(UserExistsException).FullName!, UserExistsSerializator.Serializator);
            Serializators.Add(typeof(Action).FullName!, ActionSerializator.Serializator);
            Serializators.Add(typeof(Chat).FullName!, ChatSerializator.Serializator);
            Serializators.Add(typeof(Message).FullName!, MessageSerializator.Serializator);
            Serializators.Add(typeof(UserCerdentials).FullName!, UserCerdentialsSerializator.Serializator);
            Serializators.Add(typeof(TransmitObject).FullName!, TransmitObjectSerializator.Serializator);
            Serializators.Add(typeof(User).FullName!, UserSerializator.Serializator);
            Serializators.Add(typeof(Primitive).FullName!, PrimitiveSerializator.Serializator);
            Serializators.Add(typeof(ReturnCode).FullName!, ReturnCodeSerializator.Serializator);
        }
    }
}