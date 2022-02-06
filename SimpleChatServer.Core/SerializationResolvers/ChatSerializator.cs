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
            writer.Write(data.Users.Length);

            for (int i = 0; i < data.Users.Length; i++)
                UserSerializator.Serializator.Serialize(writer, data.Users[i]);

            writer.Write(data.UsersRoles.Count);
            foreach (var userRole in data.UsersRoles)
            {
                writer.Write(userRole.Key);
                writer.Write((int)userRole.Value);
            }

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
            var usersLength = reader.ReadInt32();
            var users = new User[usersLength];
            var usersRoles = new Dictionary<int, Role>();

            for (int i = 0; i < usersLength; i++)
                users[i] = UserSerializator.Serializator.Deserialize(reader);

            var usersRolesLength = reader.ReadInt32();

            for (int i = 0; i < usersRolesLength; i++)
                usersRoles[reader.ReadInt32()] = (Role)reader.ReadInt32();

            var name = reader.ReadString();

            return new Chat(id, users, new ReadOnlyDictionary<int, Role>(usersRoles), name);
        }
    }
}