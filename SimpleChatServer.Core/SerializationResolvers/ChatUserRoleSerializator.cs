using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers;

public class ChatUserRoleSerializator : ISerializator<ChatUserRole>
{
    public static readonly ChatUserRoleSerializator Serializator = new ChatUserRoleSerializator();
    
    public void Serialize(BinaryWriter writer, object data)
    {
        Serialize(writer, (ChatUserRole)data);
    }

    public ChatUserRole Deserialize(BinaryReader reader)
    {
        return new ChatUserRole()
        {
            ChatId = reader.ReadInt32(),
            UserId = reader.ReadInt32(),
            Role = (Role)reader.ReadByte(),
            Restriction = (Restriction)reader.ReadInt32(),
        };
    }

    public void Serialize(BinaryWriter writer, ChatUserRole data)
    {
        writer.Write(data.ChatId);
        writer.Write(data.UserId);
        writer.Write((byte)data.Role);
        writer.Write((int)data.Restriction);
    }

    object ISerializator.Deserialize(BinaryReader reader)
    {
        return Deserialize(reader);
    }
}