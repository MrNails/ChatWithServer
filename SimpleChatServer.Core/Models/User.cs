using System;
using SimpleChatServer.Core.Services;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    public class User
    {
        private string m_name;

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public int Id { get; init; }

        public string Name
        {
            get => m_name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException($"{nameof(value)} cannot be empty");

                if (value.Length is < 8 or > 32)
                    throw new ArgumentOutOfRangeException(nameof(value));
                
                m_name = value;
            }
        }

        public string? Bio { get; set; }
    }
}