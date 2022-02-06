using System.Collections.Generic;

namespace SimpleChatServer.DAL.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public IEnumerable<Chat> Chats { get; set; }
    }
}