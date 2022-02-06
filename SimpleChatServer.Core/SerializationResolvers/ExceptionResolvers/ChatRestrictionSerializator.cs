using System.IO;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Core.SerializationResolvers.ExceptionResolvers
{
    public sealed class ChatRestrictionSerializator : ISerializator<ChatRestrictionException>
    {
        public static readonly ChatRestrictionSerializator Serializator = new ChatRestrictionSerializator();
        
        public void Serialize(BinaryWriter writer, ChatRestrictionException data)
        {
            writer.Write((int)data.RestrictionType);
            writer.Write(data.Message);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (ChatRestrictionException)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public ChatRestrictionException Deserialize(BinaryReader reader)
        {
            var restrictionType = (ChatRestrictionType)reader.ReadInt32();
            var msg = reader.ReadString();

            return new ChatRestrictionException(msg, restrictionType);
        }
    }
}