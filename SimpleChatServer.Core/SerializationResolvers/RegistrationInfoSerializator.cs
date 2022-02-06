using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class RegistrationInfoSerializator : ISerializator<RegistrationInfo>
    {
        public static readonly RegistrationInfoSerializator Serializator = new RegistrationInfoSerializator();
        
        public void Serialize(BinaryWriter writer, RegistrationInfo data)
        {
            writer.Write(data.Name);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (RegistrationInfo)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public RegistrationInfo Deserialize(BinaryReader reader)
        {
            var name = reader.ReadString();

            return new RegistrationInfo(name);
        }
    }
}