using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;
using SimpleChatServer.DAL;
using SimpleChatServer.Models;
using SimpleChatServer.Services.Extensions;
using Chat = SimpleChatServer.Models.Chat;
using CoreChat = SimpleChatServer.Core.Models.Chat;

namespace SimpleChatServer.Services.StaticMappableServices
{
    public static class ChatManager
    {
        private static readonly Dictionary<int, Chat> s_chats = new Dictionary<int, Chat>();

        [MapAction]
        public static async Task<ActionResponse> CreateChatAsync(string chatName, string creatorSID)
        {
            if (chatName.Length < Utilities.ChatRestriction.MinNameLength)
                return new ActionResponse(typeof(CoreChat).FullName, null, ReturnCode.ChatNameLengthLessThanMinLength);

            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var existsSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == creatorSID);

            if (existsSession == null)
                return new ActionResponse(typeof(CoreChat).FullName, null, ReturnCode.NotAuthorized);

            var existsChat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Name == chatName);

            if (existsChat != null)
                return new ActionResponse(typeof(CoreChat).FullName, null, ReturnCode.ChatWithSameNameExists);

            var chatNameParam = new SqlParameter("@chatName", chatName) { Size = 50 };
            var creatorIdParam = new SqlParameter("@creatorId", existsSession.UserId);
            var chatIdParam = new SqlParameter { ParameterName = "@chatId", Direction = ParameterDirection.Output };

            var affectedRows = await dbContext.Database.ExecuteSqlRawAsync(
                $"exec {Constants.MasterDataDB}.dbo.proc_CreateChat @chatName, @creatorId, @chatId out",
                new object[] { chatNameParam, creatorIdParam, chatIdParam });

            var users = new Dictionary<int, TcpClient>
                { { existsSession.UserId, UserManager.GetUserOrDefault(existsSession.UserId).UserClient } };
            var newChat = new Chat((int)chatIdParam.Value, users);
            var chatUserRole = new SimpleChatServer.DAL.Models.ChatUserRole(newChat.Id, existsSession.UserId);
            var coreChat = new CoreChat(newChat.Id,
                new List<ChatUserRole> { chatUserRole.AsCoreChatUserRole() },
                chatName);

            s_chats.Add(newChat.Id, newChat);

            dbContext.ChatUsersRoles.Add(chatUserRole);

            await dbContext.SaveChangesAsync();

            return new ActionResponse(typeof(CoreChat).FullName, coreChat, ReturnCode.OK);
        }

        [MapAction]
        public static async Task<ActionResponse> DeleteChatAsync(int chatId, string requesterSID)
        {
            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var existsSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == requesterSID);

            if (existsSession == null)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.NotAuthorized);

            var chat = await dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.ChatNotExists);

            dbContext.Chats.Remove(chat);

            await dbContext.SaveChangesAsync();

            return ActionResponse.VoidResponse;
        }

        [MapAction]
        public static async Task<ActionResponse> GetChatsByNameAsync(string chatName)
        {
            if (string.IsNullOrEmpty(chatName))
                return new ActionResponse(typeof(List<CoreChat>).FullName!, Enumerable.Empty<CoreChat>(),
                    ReturnCode.ArgumentIsNull);

            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var resultArr = await dbContext.Chats.Where(chat => chat.Name == chatName || chat.Name.Contains(chatName))
                .ToListAsync();

            return new ActionResponse(typeof(List<CoreChat>).FullName!, resultArr, ReturnCode.OK);
        }

        [MapAction]
        public static async Task<ActionResponse> JoinToChatAsync(int chatId, string sessionId)
        {
            var foundChat = GetChatById(chatId);

            if (foundChat == default)
                return new ActionResponse(typeof(ChatUserRole).FullName!, null, ReturnCode.ChatNotExists);

            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var existsSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == sessionId);

            if (existsSession == null)
                return new ActionResponse(typeof(ChatUserRole).FullName, null, ReturnCode.NotAuthorized);

            var user = UserManager.GetUserOrDefault(existsSession.UserId);

            var joinResult = await foundChat.JoinAsync(user.UserInfo, user.UserClient);

            if (joinResult != ReturnCode.OK)
                return new ActionResponse(typeof(ChatUserRole).FullName, null, joinResult);

            var chatUserRole = new DAL.Models.ChatUserRole(chatId, existsSession.UserId)
            {
                Role = Role.User,
                Restriction = Restriction.NoKicks | 
                              Restriction.NoAddingAdmins | 
                              Restriction.NoChangingChatInfo
            };

            dbContext.ChatUsersRoles.Add(chatUserRole);

            await dbContext.SaveChangesAsync();

            return ActionResponse.VoidResponse;
        }

        [MapAction]
        public static async Task<ActionResponse> LeaveFromChatAsync(int chatId, string sessionId)
        {
            var foundChat = GetChatById(chatId);

            if (foundChat == null)
                return new ActionResponse(typeof(void).FullName!, null, ReturnCode.ChatNotExists);
            
            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var existsSession = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == sessionId);

            if (existsSession == null)
                return new ActionResponse(typeof(void).FullName, null, ReturnCode.NotAuthorized);

            var user = UserManager.GetUserOrDefault(existsSession.UserId);

            var leaveResult = await foundChat.LeaveAsync(user.UserInfo);

            if (leaveResult != ReturnCode.OK)
                return new ActionResponse(typeof(void).FullName, null, leaveResult);
            
            dbContext.ChatUsersRoles.Remove(await dbContext.ChatUsersRoles.FirstOrDefaultAsync(cur => cur.UserId == existsSession.UserId && cur.ChatId == chatId));

            await dbContext.SaveChangesAsync();
            
            return ActionResponse.VoidResponse;
        }

        [MapAction]
        public static async Task<ActionResponse> SendMessageAsync(Message msg)
        {
            var foundChat = GetChatById(msg.InChat);

            if (foundChat == null)
                return new ActionResponse(typeof(void).FullName!, null, ReturnCode.ChatNotExists);

            await using var dbContext = new MasterDataContext(GlobalSettings.ConnectionStrings[Constants.MasterDataDB]);

            var msgSentResult = await foundChat.SendMessageAsync(msg);
            
            if (msgSentResult != ReturnCode.OK)
                return new ActionResponse(typeof(void).FullName!, null, msgSentResult);

            dbContext.Messages.Add(msg.AsDALMessage());
            
            return ActionResponse.VoidResponse;
        }

        internal static Chat? GetChatById(int chatId)
        {
            Chat chat = null;

            s_chats.TryGetValue(chatId, out chat);

            return chat;
        }
    }
}