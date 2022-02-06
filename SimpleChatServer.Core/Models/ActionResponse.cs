using System;

namespace SimpleChatServer.Core.Models
{
    public class ActionResponse
    {
        public static readonly ActionResponse VoidResponse = new ActionResponse(typeof(void).FullName!, null);
        
        public ActionResponse(string returnTypeName, object? result)
        {
            ReturnTypeName = returnTypeName;
            Result = result;
        }
        
        public string ReturnTypeName { get; }
        public object? Result { get; }

        public static bool operator ==(ActionResponse left, ActionResponse right)
        {
            return right.ReturnTypeName == left.ReturnTypeName && right.Result == left.Result;
        }
        
        public static bool operator !=(ActionResponse left, ActionResponse right)
        {
            return !(left == right);
        }
    }
}