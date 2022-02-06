using System;

namespace SimpleChatServer.Core.Services.Exceptions
{
    public sealed class ChatNotFoundException : Exception
    {
        public ChatNotFoundException(long desiredChatId) : base($"Cannot find chat by id {desiredChatId.ToString()}")
        {
            DesiredChatId = desiredChatId;
        }

        public long DesiredChatId { get; }
    }
}