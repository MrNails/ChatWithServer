using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using Action = SimpleChatServer.Core.Models.Action;

namespace SimpleChatServer.Services
{
    public static class ActionHandler
    {
        private static Dictionary<string, MethodInfo[]> _servicesMethods = new Dictionary<string, MethodInfo[]>();

        public static void AddMappableService(Type service)
        {
            var typeName = service.Name;
            if (_servicesMethods.ContainsKey(typeName))
                return;

            _servicesMethods[typeName] = service.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(method => method.GetCustomAttribute(typeof(MapActionAttribute)) != null && method.ReturnType == typeof(Task<ActionResponse>))
                .ToArray();
        }

        public static bool RemoveMappableService(Type service)
        {
            return _servicesMethods.Remove(service.Name);
        }

        public static Task<ActionResponse> PerformAction(Action action)
        {
            if (!_servicesMethods.ContainsKey(action.ServiceName))
                return Task.FromResult(ActionResponse.VoidResponse);
            
            return (Task<ActionResponse>)_servicesMethods[action.ServiceName].FirstOrDefault(method => method.Name == action.MethodName)
                ?.Invoke(null, action.Params)!;
        }
    }
}