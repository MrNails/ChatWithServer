using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Message
    {
        public Message(long id, long fromUser, string content, DateTime sendDate, long inChat)
        {
            Id = id;
            FromUser = fromUser;
            Content = content;
            SendDate = sendDate;
            InChat = inChat;
        }
        
        public long Id { get; }
        public DateTime SendDate { get; }
        
        public long FromUser { get; }
        public long InChat { get; }
        public string Content { get; }

        public int GetObjectSize()
        {
            unsafe
            {
                return sizeof(int) * 3 + sizeof(DateTime) + Content.Length * sizeof(char);
            }
        }
    }
}