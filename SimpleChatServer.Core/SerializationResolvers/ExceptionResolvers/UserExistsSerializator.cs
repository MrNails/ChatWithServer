using System.IO;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Core.SerializationResolvers.ExceptionResolvers
{
    public sealed class UserExistsSerializator : ISerializator<UserExistsException>
    {
        public static readonly UserExistsSerializator Serializator = new UserExistsSerializator();
        
        public void Serialize(BinaryWriter writer, UserExistsException data)
        {
            writer.Write(data.UserName);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (UserExistsException)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public UserExistsException Deserialize(BinaryReader reader)
        {
            return new UserExistsException(reader.ReadString());
        }
    }
}