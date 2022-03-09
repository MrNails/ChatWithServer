using System;
using System.Collections;
using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class ActionResponseSerializator : ISerializator<ActionResponse>
    {
        public static readonly ActionResponseSerializator Serializator = new ActionResponseSerializator();
        
        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (ActionResponse)data);
        }

        public ActionResponse Deserialize(BinaryReader reader)
        {
            var result = ActionResponse.VoidResponse;

            var returnCode = (ReturnCode)reader.ReadInt32();
            var returnTypeName = reader.ReadString();
            var typeName = reader.ReadString();

            if (typeName == typeof(IEnumerable).FullName)
            {
                var elemLength = reader.ReadInt32();
                object[] elems = new object[elemLength];
            
                for (int i = 0; i < elemLength; i++)
                {
                    var serializator = Utilities.Serializators[reader.ReadString()];

                    elems[i] = serializator.Deserialize(reader);
                }

                result = new ActionResponse(returnTypeName, elems, returnCode);
            }
            else
            {
                var serializator = Utilities.Serializators[reader.ReadString()];
                
                result = new ActionResponse(returnTypeName, serializator.Deserialize(reader), returnCode);
            }

            return result;
        }

        public void Serialize(BinaryWriter writer, ActionResponse data)
        {
            writer.Write((int)data.ReturnCode);
            writer.Write(data.ReturnTypeName);

            if (data.Result is IEnumerable enumerable)
            {
                var index = 0;
                var indexEnumerator = enumerable.GetEnumerator();

                while (indexEnumerator.MoveNext())
                    index++;
                
                writer.Write(typeof(IEnumerable).FullName!);
                writer.Write(index);
                
                foreach (var obj in enumerable)
                {
                    SerializeObject(writer, obj);
                }
            }
            else
            {
                SerializeObject(writer, data.Result);
            }
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        private void SerializeObject(BinaryWriter writer, object data)
        {
            ISerializator serializator;
            var parType = data.GetType();
            var parTypeFullName = 
            (
                parType.IsPrimitive || parType is Decimal || parType is String ||
                parType is Byte[] || parType is Char[]
                    ? typeof(Primitive)
                    : parType
            ).FullName;
                
            writer.Write(parTypeFullName);
            if (Utilities.Serializators.TryGetValue(parTypeFullName, out serializator))
                serializator.Serialize(writer, data);
            else
                throw new SerializationException(parType, $"Cannot serialize type {parTypeFullName}");
        }
    }
}