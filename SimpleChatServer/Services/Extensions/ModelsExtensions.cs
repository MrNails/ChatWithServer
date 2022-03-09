using DALChat = SimpleChatServer.DAL.Models.Chat;
using CoreChat = SimpleChatServer.Core.Models.Chat;
using DALChatUserRole = SimpleChatServer.DAL.Models.ChatUserRole;
using CoreChatUserRole = SimpleChatServer.Core.Models.ChatUserRole;
using DALMessage = SimpleChatServer.DAL.Models.Message;
using CoreMessage = SimpleChatServer.Core.Models.Message;

namespace SimpleChatServer.Services.Extensions;

public static class ChatExtensions
{
    public static CoreChatUserRole AsCoreChatUserRole(this DALChatUserRole current)
    {
        return new CoreChatUserRole
        {
            Restriction = current.Restriction,
            Role = current.Role,
            UserId = current.UserId,
            ChatId = current.ChatId
        };
    }
    
    public static DALChatUserRole AsDALChatUserRole(this CoreChatUserRole current)
    {
        return new DALChatUserRole
        {
            Restriction = current.Restriction,
            Role = current.Role,
            UserId = current.UserId,
            ChatId = current.ChatId
        };
    }
    
    public static CoreMessage AsCoreMessage(this DALMessage current)
    {
        return new CoreMessage(current.MessageId, current.FromUserId, current.Value, current.Sent, current.ChatId, current.MessageType);
    }
    
    public static DALMessage AsDALMessage(this CoreMessage current)
    {
        return new DALMessage
        {
            MessageId = current.Id,
            ChatId = current.InChat,
            FromUserId = current.FromUser,
            Sent = current.SendDate,
            Value = current.Content,
            MessageType = current.MessageType
        };
    }
}