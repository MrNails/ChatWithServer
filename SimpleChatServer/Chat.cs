#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Serilog;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Exceptions;
using SimpleChatServer.Services.Extensions;

namespace SimpleChatServer
{
    public sealed class Chat
    {
        private static readonly int m_maxUsers = 10;

        private readonly long m_id;

        private readonly ReadOnlyCollection<ServerUser> m_readOnlyUsers;
        private readonly ReadOnlyDictionary<long, Role> m_readOnlyUsersRoles;

        private readonly List<ServerUser> m_users;
        private readonly List<Message> m_messages;
        private readonly Dictionary<long, Role> m_usersRoles;

        private readonly User m_chatUser;

        private bool m_isInternalDelete;
        private string m_name;

        public Chat(long id, ServerUser owner, string name)
        {
            m_users = new List<ServerUser>() { owner };
            m_messages = new List<Message>();
            m_chatUser = new User(-1, string.Empty, string.Empty);
            m_usersRoles = new Dictionary<long, Role>() { { -1, Role.Server }, { owner.UserInfo.Id, Role.Owner } };

            m_readOnlyUsers = new ReadOnlyCollection<ServerUser>(m_users);
            m_readOnlyUsersRoles = new ReadOnlyDictionary<long, Role>(m_usersRoles);

            m_name = name;

            m_id = id;
        }

        public long Id => m_id;

        public string Name => m_name;

        public ReadOnlyCollection<ServerUser> Users => m_readOnlyUsers;

        public ReadOnlyDictionary<long, Role> UsersRoles => m_readOnlyUsersRoles;
        
        public async Task JoinAsync(ServerUser user)
        {
            if (m_users.Count + 1 == m_maxUsers)
                throw new ChatRestrictionException("Chat have max people count.", ChatRestrictionType.MaxPeopleRestriction);

            if (m_users.FirstOrDefault(u => u.UserInfo.Id == user.UserInfo.Id).UserInfo != null)
                throw new UserExistsException(user.UserInfo.Name);

                m_users.Add(user);
            m_usersRoles[user.UserInfo.Id] = Role.User;

            await NotifyAllAsync($"{user.UserInfo.Name} joined");
        }

        public async Task LeaveAsync(long userId)
        {
            var foundUser = m_users.FirstOrDefault(user => user.UserInfo.Id == userId);

            if (foundUser.UserInfo != null)
            {
                m_users.Remove(foundUser);
                m_usersRoles.Remove(foundUser.UserInfo.Id);
                
                await NotifyAllAsync($"{foundUser.UserInfo.Name} left");
            }
        }

        public void DisconnectAll()
        {
            //await 
        }

        public Task SendMessageAsync(in Message message)
        {
            var tasks = new Task[m_users.Count - 1];

            for (int i = 0, taskIndex = 0; i < m_users.Count; i++)
            {
                if (m_users[i].UserInfo.Id != message.FromUser)
                {
                    tasks[taskIndex] = Sender.SendObjectAsync(m_users[i].UserClient, message, MessageSerializator.Serializator);
                    taskIndex++;
                }
            }

            return Task.WhenAll(tasks);
        }

        public ServerUser? GetUserById(long userId)
        {
            return m_users.FirstOrDefault(u => u.UserInfo.Id == userId);
        }
        
        private async Task NotifyAllAsync(string msg)
        {
            var lastMsgId = m_messages.LastOrDefault().Id;
            var message = new Message(lastMsgId, m_chatUser.Id, msg, DateTime.Now, m_id);

            // await Sender.SendObjectAsync(m_client, message, MessageSerializator.Serializator);
            await Task.CompletedTask;
        }

    }
}