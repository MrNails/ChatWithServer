using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Action
    {
        public Action(string serviceName, string methodName, object[] @params)
        {
            ServiceName = serviceName;
            MethodName = methodName;
            Params = @params;
        }
        
        public string ServiceName { get; }
        public string MethodName { get; }
        public object[] Params { get; }
    }
}