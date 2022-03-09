namespace SimpleChatServer.DAL.Models;

public class Session
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime Created { get; set; }
}