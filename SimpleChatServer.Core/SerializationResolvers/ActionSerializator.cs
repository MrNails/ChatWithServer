using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class ActionSerializator : ISerializator<Action>
    {
        public static readonly ActionSerializator Serializator = new ActionSerializator();

        public void Serialize(BinaryWriter writer, Action data)
        {
            writer.Write(data.ServiceName);
            writer.Write(data.MethodName);
            writer.Write(data.Params.Length);

            foreach (var par in data.Params)
            {
                ISerializator serializator;
                var parType = par.GetType();
                var parTypeFullName = 
                (
                    parType.IsPrimitive || parType is Decimal || parType is String ||
                    parType is Byte[] || parType is Char[]
                    ? typeof(Primitive)
                    : parType
                ).FullName;
                
                writer.Write(parTypeFullName);
                if (Utilities.Serializators.TryGetValue(parTypeFullName, out serializator))
                    serializator.Serialize(writer, par);
                else
                    throw new SerializationException(parType, $"Cannot serialize type {parTypeFullName}");
            }
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (Action)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public Action Deserialize(BinaryReader reader)
        {
            var serviceName = reader.ReadString();
            var methodName = reader.ReadString();
            var paramsLength = reader.ReadInt32();

            object[] _params = new object[paramsLength];
            
            for (int i = 0; i < paramsLength; i++)
            {
                var serializator = Utilities.Serializators[reader.ReadString()];

                _params[i] = serializator.Deserialize(reader);
            }

            return new Action(serviceName, methodName, _params);
        }
    }
}