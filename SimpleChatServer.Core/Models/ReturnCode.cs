using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Models
{
    public enum ReturnCode
    {
        OK = 0,
        
        ArgumentIsNull = 101,
        CollectionIsEmpty = 102,
        ArgumentsNotMatched = 103,
        ObjectFilledIncorrect = 104,

        UserNotFound = 201,
        UserWithSameNameExists = 202,
        UserAlreadyInChat = 203,
        UserPasswordIsInvalid = 204,
        
        ChatNotExists = 250,
        ChatWithSameNameExists = 251,
        ChatNameLengthLessThanMinLength = 252,
        ChatNameLengthGreaterThanMaxLength = 253,

        NotAuthorized = 301,
        
        InternalServerError = 500,
    }
}