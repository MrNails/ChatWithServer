using System.IO;

namespace SimpleChatServer.Core.Services
{
    public interface ISerializator
    {
        void Serialize(BinaryWriter writer, object data);
        object Deserialize(BinaryReader reader);
    }
    
    public interface ISerializator<T> : ISerializator
    {
        void Serialize(BinaryWriter writer, T data);
        T Deserialize(BinaryReader reader);
    }
}