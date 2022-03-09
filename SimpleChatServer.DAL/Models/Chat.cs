using System.Collections.Generic;

namespace SimpleChatServer.DAL.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        // public IEnumerable<User> Users { get; set; }
        //
        // public IEnumerable<Message> Messages { get; set; }
        //
        // public IEnumerable<ChatUsersRole> ChatUsersRoles { get; set; }
    }
}