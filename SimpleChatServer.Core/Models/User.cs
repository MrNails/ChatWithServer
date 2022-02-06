using System;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    public class User : ISender
    {
        public User() : this(-1)  { }
        public User(long id)
        {
            Id = id;
            Name = string.Empty;
        }

        public long Id { get; init; }
        public string Name { get; set; }
    }
}