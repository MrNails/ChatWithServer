using System.Collections.Generic;

namespace SimpleChatServer.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // public IEnumerable<Chat> Chats { get; set; }
        // public IEnumerable<Message> Messages { get; set; }
    }
}