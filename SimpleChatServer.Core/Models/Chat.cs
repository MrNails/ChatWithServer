using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private SortedDictionary<int, Message> m_messages;
        private List<ChatUserRole> m_usersAndRoles;

        public event MessageDelegate? MessageReceived;
        
        public event UserDelegate? UserJoin;
        public event UserDelegate? UserLeave;

        public Chat(int id, IEnumerable<ChatUserRole> usersAndRoles, string name)
        {
            if (usersAndRoles.FirstOrDefault() == null)
                throw new ArgumentException("There must be at least one user in chat.", nameof(usersAndRoles));
            
            Name = name;
            Id = id;

            m_messages = new SortedDictionary<int, Message>();
            m_usersAndRoles = usersAndRoles.ToList();

            MessageReceived = null;
        }

        public int Id { get; }

        public string Name { get; }
        
        public ReadOnlyCollection<ChatUserRole> UsersAndRoles => m_usersAndRoles.AsReadOnly();

        public ReadOnlyDictionary<int, Message> Messages => new ReadOnlyDictionary<int, Message>(m_messages);

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

        public async Task LoadMessagesAsync(DateTime from)
        {
            var firstMsg = m_messages.FirstOrDefault();
            
            if (firstMsg.Value == null || firstMsg.Value.SendDate < from)
                return;
            
            await Task.CompletedTask;
        }
        
        internal void ReceivedMessages(Message[] messages)
        {
            if (messages.Length == 0)
                return;

            for (int i = 0; i < messages.Length; i++)
                m_messages.TryAdd(messages[i].Id, messages[i]);

            MessageReceived?.Invoke(this, messages);
        }

        internal void JoinedUser(ChatUserRole joinedUser)
        {
            m_usersAndRoles.Add(joinedUser);
            UserJoin?.Invoke(this, joinedUser.UserId);
        }

        private async Task<Message> SendMessageInternal(string message, User from)
        {
            var msg = new Message(m_messages.LastOrDefault().Value.Id, from.Id, message, DateTime.Now, Id, MessageType.Image);

            await Sender.SendObjectAsync(GlobalSettings.CurrentClient, msg, MessageSerializator.Serializator);

            return msg;
        }
    }
}