using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Services.StaticMappableServices
{
    public static class UserManager
    {
        private static readonly Dictionary<long, ServerUser> s_users = new Dictionary<long, ServerUser>();

        [MapAction]
        public static Task<ActionResponse> CreateUser(RegistrationInfo registrationInfo, TcpClient client)
        {
            if (s_users.FirstOrDefault(u => u.Value.UserInfo.Name == registrationInfo.Name).Value.UserInfo != null)
                return Task.FromResult(new ActionResponse(typeof(UserExistsException).FullName!,new UserExistsException(registrationInfo.Name)));

            var userMaxId = GetUserMaxId() + 1;
            
            s_users.Add(userMaxId, new ServerUser(client, new User(userMaxId, registrationInfo.Name, registrationInfo.BIO) ));

            return Task.FromResult(ActionResponse.VoidResponse);
        }

        [MapAction]
        public static Task<ActionResponse> RemoveUser(long userId)
        {
            return Task.FromResult(new ActionResponse(typeof(bool).FullName!, s_users.Remove(userId)));
        }

        [MapAction]
        public static Task<ActionResponse> LoginUser()
        {
            // var foundUser = s_users => s_users.
            return Task.FromResult(ActionResponse.VoidResponse);
        }

        [MapAction]
        public static Task<ActionResponse> GetUsers(long[] ids)
        {
            if (ids == null)
                return Task.FromResult(new ActionResponse(typeof(ReturnCode).FullName!, ReturnCode.ArgumentIsNull));
            
            if (ids.Length == 0)
                return Task.FromResult(new ActionResponse(typeof(ReturnCode).FullName!, Enumerable.Empty<User>()));
            
            return Task.Run(() =>
            {
                var result = s_users.Where(u => ids.Contains(u.Value.UserInfo.Id))
                    .ToArray();
                return new ActionResponse(result.GetType().FullName!, result);
            });
        }

        private static long GetUserMaxId() => s_users.Max(user => user.Key);

        public static ServerUser GetUserById(long userId)
        {
            ServerUser sUser;
            s_users.TryGetValue(userId, out sUser);
            
            return sUser;
        }
    }
}