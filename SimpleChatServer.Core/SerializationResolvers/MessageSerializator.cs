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
            writer.Write(message.SendDate.Year);
            writer.Write(message.SendDate.Month);
            writer.Write(message.SendDate.Day);
            writer.Write(message.SendDate.Hour);
            writer.Write(message.SendDate.Minute);
            writer.Write(message.SendDate.Second);
            writer.Write(message.FromUser);
            writer.Write(message.InChat);
            writer.Write(message.Content);
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
            var id = reader.ReadInt64();
            var year = reader.ReadInt32();
            var month = reader.ReadInt32();
            var day = reader.ReadInt32();
            var hour = reader.ReadInt32();
            var minute = reader.ReadInt32();
            var second = reader.ReadInt32();
            var fromUser = reader.ReadInt64();
            var inChat =  reader.ReadInt64();
            var content = reader.ReadString();

            return new Message(id, fromUser, content, new DateTime(year, month, day, hour, minute, second), inChat);
        }
    }
}