namespace SimpleChatServer.DAL.Models;

public class ChatUsersRole
{
    public long ChatId { get; set; }
    public Chat Chat { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }

    public Restriction Restriction { get; set; }
    public Role Role { get; set; }
}