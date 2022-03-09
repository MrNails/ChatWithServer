using System;
using System.Threading.Tasks;

namespace SimpleChatServer.Core.Models
{
    /// <summary>
    /// Represent server response
    /// </summary>
    public class ActionResponse
    {
        /// <summary>
        /// Void server response for method, which don't have response
        /// </summary>
        public static readonly ActionResponse VoidResponse = new ActionResponse(typeof(void).FullName!, null, ReturnCode.OK);

        /// <summary>
        /// Void server response returning in task for async method, which don't have response
        /// </summary>
        public static readonly Task<ActionResponse> VoidTaskResponse = Task.FromResult(VoidResponse);
        
        public ActionResponse(string returnTypeName, object? result, ReturnCode returnCode)
        {
            ReturnTypeName = returnTypeName;
            Result = result;
            ReturnCode = returnCode;
        }
        
        public ReturnCode ReturnCode { get; }
        
        public string ReturnTypeName { get; }

        public object? Result { get; }

        public static bool operator ==(ActionResponse left, ActionResponse right)
        {
            return right.ReturnTypeName == left.ReturnTypeName && 
                   right.Result == left.Result &&
                   right.ReturnCode == left.ReturnCode;
        }
        
        public static bool operator !=(ActionResponse left, ActionResponse right)
        {
            return !(left == right);
        }
    }
}