using System;
using System.Globalization;
using System.IO;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.SerializationResolvers
{
    public class MessageSerializator : ISerializator<Message>
    {
        public static readonly MessageSerializator Serializator = new MessageSerializator();
        
        public void Serialize(BinaryWriter writer, Message message)
        {
            writer.Write(message.Id);
            writer.Write(message.SendDate.Ticks);
            writer.Write(message.FromUser);
            writer.Write(message.InChat);
            writer.Write(message.Content);
            writer.Write((byte)message.MessageType);
        }

        public void Serialize(BinaryWriter writer, object data)
        {
            Serialize(writer, (Message)data);
        }

        object ISerializator.Deserialize(BinaryReader reader)
        {
            return Deserialize(reader);
        }

        public Message Deserialize(BinaryReader reader)
        {
            var id = reader.ReadInt32();
            var fromDateTicks = reader.ReadInt64();
            var fromUser = reader.ReadInt32();
            var inChat =  reader.ReadInt32();
            var content = reader.ReadString();
            var msgType = (MessageType)reader.ReadByte();

            return new Message(id, fromUser, content, new DateTime(fromDateTicks), inChat, msgType);
        }
    }
}