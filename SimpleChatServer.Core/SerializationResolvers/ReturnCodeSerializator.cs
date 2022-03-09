using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers;

public class ReturnCodeSerializator : ISerializator<ReturnCode>
{
    public static readonly ReturnCodeSerializator Serializator = new ReturnCodeSerializator();
    
    public void Serialize(BinaryWriter writer, object data)
    {
        Serialize(writer, (ReturnCode)data);
    }

    public ReturnCode Deserialize(BinaryReader reader)
    {
        return (ReturnCode)reader.ReadInt32();
    }

    public void Serialize(BinaryWriter writer, ReturnCode data)
    {
        writer.Write((int)data);
    }

    object ISerializator.Deserialize(BinaryReader reader)
    {
        return Deserialize(reader);
    }
}