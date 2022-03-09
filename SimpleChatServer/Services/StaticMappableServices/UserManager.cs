using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Core.Models;
using SimpleChatServer.DAL;
using SimpleChatServer.DAL.Models;
using SimpleChatServer.Models;
using SimpleChatServer.Services.Helpers;
using User = SimpleChatServer.Core.Models.User;

namespace SimpleChatServer.Services.StaticMappableServices
{
    public static class UserManager
    {
        private static readonly Dictionary<int, ServerUser> s_users;

        static UserManager()
        {
            s_users = new Dictionary<int, ServerUser>();
        }

        [MapAction]
        public static async Task<ActionResponse> CreateUser(UserCerdentials userCerdentials, TcpClient client)
        {
            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var user = await dbContext.Users.Where(u => u.Login == userCerdentials.Login)
                .ToListAsync();

            if (user.Count == 0)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.UserWithSameNameExists);

            var salt = RandomNumberGenerator.GetBytes(Constants.SaltLength);
            var encryptedPwd = CryptoHelper.EncryptData(userCerdentials.Password, salt);

            if (encryptedPwd.Length == 0)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.ObjectFilledIncorrect);

            var loginParam = new SqlParameter("@login", userCerdentials.Login) { Size = 40 };
            var pwdParam = new SqlParameter("@password", Encoding.UTF8.GetString(encryptedPwd)) { Size = 64 };
            var saltParam = new SqlParameter("@salt", Encoding.UTF8.GetString(salt)) { Size = 32 };
            var userIdParam = new SqlParameter { ParameterName = "@userId", Direction = ParameterDirection.Output };

            var affectedRows = await dbContext.Database.ExecuteSqlRawAsync(
                $"exec {Constants.MasterDataDB}.dbo.proc_CreateUser @login, @password, @salt, @userId out",
                new object[] { loginParam, pwdParam, saltParam, userIdParam });

            var createdUserId = (int)userIdParam.Value;

            s_users.Add(createdUserId,
                new ServerUser(client,
                    new DAL.Models.User { Id = createdUserId, Login = userCerdentials.Login })
            );

            return new ActionResponse(typeof(void).FullName, null, ReturnCode.OK);
        }

        [MapAction]
        public static async Task<ActionResponse> DeleteAccount(string requesterId)
        {
            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var existsSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == requesterId);

            if (existsSession == null)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.NotAuthorized);

            var user = await dbContext.Users.FirstOrDefaultAsync(c => c.Id == existsSession.UserId);

            if (user == null)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.ChatNotExists);

            s_users.Remove(existsSession.UserId);

            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return new ActionResponse(typeof(void).FullName, null, ReturnCode.OK);
        }

        [MapAction]
        public static async Task<ActionResponse> LoginUser(UserCerdentials cerdentials, TcpClient client)
        {
            if (cerdentials.IsForRegistration)
                return new ActionResponse(typeof(string).FullName, string.Empty, ReturnCode.ObjectFilledIncorrect);

            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Login == cerdentials.Login);

            if (user == null)
                return new ActionResponse(typeof(string).FullName, string.Empty, ReturnCode.UserNotFound);

            var encryptedPwd =
                Encoding.UTF8.GetString(CryptoHelper.EncryptData(cerdentials.Password,
                    Encoding.UTF8.GetBytes(user.Salt)));

            if (user.Password != encryptedPwd)
                return new ActionResponse(typeof(string).FullName, string.Empty, ReturnCode.UserPasswordIsInvalid);

            var existingSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.UserId == user.Id);
            var expireDate = DateTime.UtcNow.AddMinutes(GlobalSettings.AuthOptions.Lifetime);

            if (existingSession == null)
            {
                existingSession = new Session
                {
                    UserId = user.Id,
                    Token = CryptoHelper.CreateJWT(user),
                    Created = DateTime.UtcNow,
                    ExpireDate = expireDate
                };

                dbContext.Sessions.Add(existingSession);
            }
            else
                existingSession.ExpireDate = expireDate;

            await dbContext.SaveChangesAsync();

            if (!s_users.ContainsKey(existingSession.UserId))
                s_users.Add(existingSession.UserId,
                    new ServerUser(client,
                        new DAL.Models.User { Id = existingSession.UserId, Login = cerdentials.Login })
                );

            return new ActionResponse(typeof(string).FullName, existingSession.Token, ReturnCode.OK);
        }

        [MapAction]
        public static async Task<ActionResponse> GetUsers(string login)
        {
            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var foundUsers = await dbContext.Users.Where(u => u.Login.Contains(login))
                .ToListAsync();

            return new ActionResponse(typeof(List<User>).FullName, foundUsers, ReturnCode.OK);
        }

        internal static ServerUser? GetUserOrDefault(int userId)
        {
            ServerUser? client;

            s_users.TryGetValue(userId, out client);

            return client;
        }

        internal static bool DisconnectUser(TcpClient client)
        {
            var existsUser = s_users.Values.FirstOrDefault(u => u.UserClient == client);

            if (existsUser == null)
                return false;

            return s_users.Remove(existsUser.UserInfo.Id);
        }
    }
}