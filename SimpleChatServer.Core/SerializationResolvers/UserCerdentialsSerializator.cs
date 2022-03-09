using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class UserCerdentialsSerializator : ISerializator<UserCerdentials>
    {
        public static readonly UserCerdentialsSerializator Serializator = new UserCerdentialsSerializator();
        
        public void Serialize(BinaryWriter writer, UserCerdentials data)
        {
            writer.Write(data.IsForRegistration);
            writer.Write(data.Login);
            writer.Write(data.Password.Length);
            writer.Write(data.Password);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (UserCerdentials)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public UserCerdentials Deserialize(BinaryReader reader)
        {
            var isForCerdentials = reader.ReadBoolean();
            var name = reader.ReadString();
            var passwordLength = reader.ReadInt32();
            var password = reader.ReadBytes(passwordLength);
            
            return new UserCerdentials(name, password, isForCerdentials);
        }
    }
}