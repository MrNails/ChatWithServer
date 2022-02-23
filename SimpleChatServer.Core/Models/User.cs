using System;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    public class User
    {
        public User(long id, string name, string bio)
        {
            Id = id;
            Name = name;
            Bio = bio;
        }
        
        public long Id { get; init; }
        public string Name { get; set; }
        public string Bio { get; set; }
    }
}