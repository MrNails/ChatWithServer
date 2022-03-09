using SimpleChatServer.Core.Models;

namespace SimpleChatServer.DAL.Models;

public class Message
{
    /// <summary>
    /// Message Id
    /// </summary>
    public int MessageId { get; set; }

    /// <summary>
    /// Chat, where this message located
    /// </summary>
    public int ChatId { get; set; }
    
    /// <summary>
    /// Chat, where this message located
    /// </summary>
    public Chat Chat { get; set; }
    
    /// <summary>
    /// Type of message. Can be text or image
    /// </summary>
    public MessageType MessageType { get; set; }
    
    /// <summary>
    /// Message value. Contains either message text or link to message
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// Date of message sent
    /// </summary>
    public DateTime Sent { get; set; }
    
    /// <summary>
    /// Date of message modified
    /// </summary>
    public DateTime? Modified { get; set; }
    
    /// <summary>
    /// From User
    /// </summary>
    public int FromUserId { get; set; }
    
    /// <summary>
    /// From User
    /// </summary>
    public User User { get; set; }
}