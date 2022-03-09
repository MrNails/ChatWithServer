using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.DAL.Models;

public class ChatUserRole
{
    public ChatUserRole() { }

    public ChatUserRole(int chatId, int userId)
    {
        ChatId = chatId;
        UserId = userId;

        Restriction = Restriction.None;
        Role = Role.Owner;
    }
    
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }

    public Restriction Restriction { get; set; }
    public Role Role { get; set; }
}