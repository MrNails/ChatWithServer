using System;

namespace SimpleChatServer.Core.Services.Exceptions
{
    public class UserDisconnectedException : Exception
    {
        public UserDisconnectedException(string message, int chatId, int userId) : base(message)
        {
            ChatId = chatId;
            UserId = userId;
        }
        
        public int ChatId { get; }
        public int UserId { get; }
    }
}