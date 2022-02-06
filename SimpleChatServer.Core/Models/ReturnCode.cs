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
        
        UserNotFound = 201,
        UserWithSameNameExists = 202,
        
        InternalServerError = 500,
    }
}