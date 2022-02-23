using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Delegates;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class Chat
    {
        private long m_id;
        private User[] m_users;
        private string m_name;

        private List<Message> _messages;
        private ReadOnlyDictionary<int, Role> _usersRoles;

        public event MessageDelegate? MessageReceived;

        public Chat(long id, User[] users, ReadOnlyDictionary<int, Role> usersRoles, string name)
        {
            m_users = users;
            m_name = name;
            m_id = id;

            _messages = new List<Message>();
            _usersRoles = usersRoles;
            
            MessageReceived = null;
        }

        public long Id => m_id;
        public User[] Users => m_users;

        public ReadOnlyDictionary<int, Role> UsersRoles => _usersRoles;

        public string Name => m_name;

        public async Task JoinAsync(User user)
        {
            // await SendMessageInternal($"{user.Name} joined to chat", m_chatUser);
            await Task.CompletedTask;
        }

        public async Task LeaveAsync(User user)
        {
            // await SendMessageInternal($"{user.Name} left from chat", m_chatUser);
            await Task.CompletedTask;
        }

        public Task<Message> SendMessageAsync(string message, User from)
        {
            return SendMessageInternal(message, from);
        }

        private async Task<Message> SendMessageInternal(string message, User from)
        {
            var msg = new Message(_messages.LastOrDefault().Id, from.Id, message, DateTime.Now, Id);
            
            await Sender.SendObjectAsync(GlobalSettings.CurrentClient, msg, MessageSerializator.Serializator);

            return msg;
        }
    }
}