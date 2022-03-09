using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Message
    {
        public Message(int id, int fromUser, string content, DateTime sendDate, int inChat, MessageType messageType)
        {
            Id = id;
            FromUser = fromUser;
            Content = content;
            SendDate = sendDate;
            InChat = inChat;
            MessageType = messageType;
        }
        
        public int Id { get; }
        public DateTime SendDate { get; }
        
        public int FromUser { get; }
        public int InChat { get; }
        public MessageType MessageType { get; }
        public string Content { get; }
    }
}