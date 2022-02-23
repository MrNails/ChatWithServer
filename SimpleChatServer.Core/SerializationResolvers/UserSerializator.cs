using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class UserSerializator : ISerializator<User>
    {
        public static readonly UserSerializator Serializator = new UserSerializator();
        
        public void Serialize(BinaryWriter writer, User data)
        {
            writer.Write(data.Id);
            writer.Write(data.Name);
            writer.Write(data.Bio);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (User)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public User Deserialize(BinaryReader reader)
        {
            return new User(reader.ReadInt64(), reader.ReadString(), reader.ReadString());
        }
    }
}