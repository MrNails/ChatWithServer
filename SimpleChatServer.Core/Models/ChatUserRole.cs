using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.Models
{
    public class ChatUserRole
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public Role Role { get; set; }
        public Restriction Restriction { get; set; }
    }
}