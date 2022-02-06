using System;

namespace SimpleChatServer.Core.Services.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException(string userName) : base($"User with nick {userName} already exists.")
        {
            UserName = userName;
        }
        
        public string UserName { get; }
    }
}