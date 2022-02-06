using System;

namespace SimpleChatServer.Core.Services.Exceptions
{
    public enum ChatRestrictionType
    {
        /// <summary>
        /// When chat have max people and was attempt to join chosen chat
        /// </summary>
        MaxPeopleRestriction,
        
        /// <summary>
        /// When user was banned in chosen chat
        /// </summary>
        AccessDeniedRestriction
    }
    
    public sealed class ChatRestrictionException : Exception
    {
        public ChatRestrictionException(string message, ChatRestrictionType restrictionType) : base(message)
        {
            RestrictionType = restrictionType;
        }

        public ChatRestrictionType RestrictionType { get; }
    }
}