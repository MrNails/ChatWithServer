using System.IO;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Core.SerializationResolvers.ExceptionResolvers
{
    public sealed class ChatNotFoundSerializator : ISerializator<ChatNotFoundException>
    {
        public static readonly ChatNotFoundSerializator Serializator = new ChatNotFoundSerializator();
        
        public void Serialize(BinaryWriter writer, ChatNotFoundException data)
        {
            writer.Write(data.DesiredChatId);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (ChatNotFoundException)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public ChatNotFoundException Deserialize(BinaryReader reader)
        {
            return new ChatNotFoundException(reader.ReadInt32());
        }
    }
}