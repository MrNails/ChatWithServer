using System;
using System.IO;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class TransmitObjectSerializator : ISerializator<TransmitObject>
    {
        public static readonly TransmitObjectSerializator Serializator = new TransmitObjectSerializator();
        
        public void Serialize(BinaryWriter writer, TransmitObject data)
        {
            var idInBytes = data.Id.ToByteArray();
            
            writer.Write(data.ContentSize);
            writer.Write(idInBytes.Length);
            writer.Write(idInBytes);
            writer.Write(data.TransmitType);
            writer.Write(data.Content);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (TransmitObject)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public TransmitObject Deserialize(BinaryReader reader)
        {
            var contentSize = reader.ReadInt32();
            var idLength = reader.ReadInt32();
            var id = reader.ReadBytes(idLength);
            var transmitType = reader.ReadString();
            var content = reader.ReadBytes(contentSize);

            return new TransmitObject(transmitType, content, contentSize, new Guid(id));
        }
    }
}