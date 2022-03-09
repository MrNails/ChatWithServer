using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class ChatSerializator : ISerializator<Chat>
    {
        public static readonly ChatSerializator Serializator = new ChatSerializator();

        public void Serialize(BinaryWriter writer, Chat data)
        {
            writer.Write(data.Id);

            writer.Write(data.UsersAndRoles.Count);
            foreach (var userRole in data.UsersAndRoles)
                ChatUserRoleSerializator.Serializator.Serialize(writer, userRole);

            writer.Write(data.Name);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (Chat)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public Chat Deserialize(BinaryReader reader)
        {
            var id = reader.ReadInt32();

            var usersRolesLength = reader.ReadInt32();
            var userRoles = new List<ChatUserRole>(usersRolesLength);

            for (int i = 0; i < usersRolesLength; i++)
                userRoles.Add(ChatUserRoleSerializator.Serializator.Deserialize(reader));

            var name = reader.ReadString();

            return new Chat(id, userRoles, name);
        }
    }
}