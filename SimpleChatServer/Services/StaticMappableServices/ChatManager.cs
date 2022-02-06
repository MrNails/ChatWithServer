using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;

namespace SimpleChatServer.Services.StaticMappableServices
{
    public static class ChatManager
    {
        private static readonly Dictionary<long, Chat> s_chats = new Dictionary<long, Chat>();

        private static int chatIdCount = 0;
        
        [MapAction]
        public static Task<ActionResponse> CreateChatAsync(string chatName, ServerUser owner)
        {
            var newChat = new Chat(++chatIdCount, owner, chatName);

            s_chats.Add(newChat.Id, newChat);

            return Task.FromResult(new ActionResponse(typeof(Chat).FullName!, newChat));
        }

        [MapAction]
        public static Task<ActionResponse> DeleteChatAsync(long chatId, long requesterId)
        {
            var foundChat = GetChatById(chatId);
            Role requesterRole;

            if (foundChat != null && foundChat.UsersRoles.TryGetValue(requesterId, out requesterRole) && requesterRole == Role.Owner)
            {
                foundChat.DisconnectAll();
                return Task.FromResult(new ActionResponse(typeof(bool).FullName!, s_chats.Remove(chatId)));
            }

            return Task.FromResult(new ActionResponse(typeof(bool).FullName!, false));
        }

        [MapAction]
        public static Task<ActionResponse> GetChatsByNameAsync(string chatName)
        {
            if (string.IsNullOrEmpty(chatName))
                return Task.FromResult(new ActionResponse(typeof(Chat[]).FullName!, Array.Empty<Chat>()));

            var resultArr = s_chats.Values.Where(chat => chat.Name == chatName || chat.Name.Contains(chatName))
                                               .ToArray();

            return Task.FromResult(new ActionResponse(typeof(Chat[]).FullName!, resultArr));
        }

        [MapAction]
        public static Task<ActionResponse> JoinToChatAsync(long chatId, ServerUser user)
        {
            var foundChat = GetChatById(chatId);

            if (foundChat == default)
                return Task.FromResult(new ActionResponse(typeof(Exception).FullName!, new ChatNotFoundException(chatId)));

            return foundChat.JoinAsync(user).ContinueWith(res =>
            {
                ActionResponse result = null;
                if (res.IsFaulted)
                    result = new ActionResponse(typeof(Exception).FullName!, res.Exception);
                else if (res.IsCanceled)
                    result = new ActionResponse(typeof(Exception).FullName!, new TaskCanceledException("Task was canceled."));
                else
                    result = ActionResponse.VoidResponse;
                
                return result;
            });
        }

        [MapAction]
        public static Task<ActionResponse> LeaveFromChatAsync(long chatId, long userId)
        {
            var foundChat = GetChatById(chatId);

            if (foundChat == default)
                return Task.FromResult(new ActionResponse(typeof(Exception).FullName!, new ChatNotFoundException(chatId)));
            
            return foundChat.LeaveAsync(userId).ContinueWith(res =>
            {
                ActionResponse result = null;
                if (res.IsFaulted)
                    result = new ActionResponse(typeof(Exception).FullName!, res.Exception);
                else if (res.IsCanceled)
                    result = new ActionResponse(typeof(Exception).FullName!, new TaskCanceledException("Task was canceled."));
                else
                    result = ActionResponse.VoidResponse;
                
                return result;
            });
        }

        [MapAction]
        public static Task<ActionResponse> SendMessageAsync(in Message msg)
        {
            var foundChat = GetChatById(msg.InChat);

            if (foundChat == null)
                return Task.FromResult(new ActionResponse(typeof(Exception).FullName!, new ChatNotFoundException(msg.InChat)));

            return foundChat.SendMessageAsync(msg).ContinueWith(res =>
            {
                ActionResponse result = null;
                if (res.IsFaulted)
                    result = new ActionResponse(typeof(Exception).FullName!, res.Exception);
                else if (res.IsCanceled)
                    result = new ActionResponse(typeof(Exception).FullName!, new TaskCanceledException("Task was canceled."));
                else
                    result = ActionResponse.VoidResponse;
                
                return result;
            });
        }

        private static long GetChatMaxId() => s_chats.Max(chat => chat.Key);

        public static Chat? GetChatById(long chatId)
        {
            Chat chat = null;

            s_chats.TryGetValue(chatId, out chat);
            
            return chat;
        }
    }
}