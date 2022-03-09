using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using SimpleChatServer.Core.Models;
using SimpleChatServer.Core.SerializationResolvers;
using SimpleChatServer.Core.Services;
using SimpleChatServer.Core.Services.Delegates;
using User = SimpleChatServer.DAL.Models.User;
using DALChat = SimpleChatServer.DAL.Models.Chat;
using CoreChat = SimpleChatServer.Core.Models.Chat;

namespace SimpleChatServer.Models;

internal sealed class Chat
{
    private readonly Dictionary<int, TcpClient> m_users;
    private readonly ReadOnlyDictionary<int, TcpClient> m_readonlyUsers;

    public Chat(int id, IDictionary<int, TcpClient> users)
    {
        Id = id;

        m_users = new Dictionary<int, TcpClient>(users);
        m_readonlyUsers = new ReadOnlyDictionary<int, TcpClient>(m_users);
    }

    public int Id { get; }

    public ReadOnlyDictionary<int, TcpClient> Users => m_readonlyUsers;

    public Task<ReturnCode> JoinAsync(User user, TcpClient userClient)
    {
        if (m_users.ContainsKey(user.Id))
            return Task.FromResult(ReturnCode.UserAlreadyInChat);

        m_users.Add(user.Id, userClient);
        var message = new Message(-1, user.Id, $"User {user.Login} joined to the chat", DateTime.UtcNow, Id, MessageType.Text);

        return NotifyAll(message)
            .ContinueWith(r => r.IsCompleted ? ReturnCode.OK : ReturnCode.InternalServerError);
    }

    public Task<ReturnCode> LeaveAsync(User user)
    {
        var returnCode = m_users.Remove(user.Id) ? ReturnCode.OK : ReturnCode.UserNotFound;
        var msg = new Message(-1, user.Id, $"User {user.Login} left from the chat", DateTime.UtcNow, Id, MessageType.Text);
        
        return NotifyAll(msg)
            .ContinueWith(r => r.IsCompleted ? returnCode : ReturnCode.InternalServerError);;
    }
    
    public Task<ReturnCode> SendMessageAsync(Message message)
    {
        return NotifyAll(message)
            .ContinueWith(r => r.IsCompleted ? ReturnCode.OK : ReturnCode.InternalServerError);
    }

    private Task NotifyAll(Message message)
    {
        var tasks = new List<Task<Guid>>(m_users.Count);

        foreach (var user in m_users)
            if (user.Key != message.FromUser)
                tasks.Add(Sender.SendObjectAsync(user.Value, message, MessageSerializator.Serializator));

        return Task.WhenAll(tasks)
            .ContinueWith(HandleSentMessages);
    }

    private void HandleSentMessages(Task<Guid[]> result)
    {
        if (!result.IsCompleted)
            return;

        Serilog.Log.Information("\nSent messages:\n");

        foreach (var guid in result.Result)
            Serilog.Log.Information($"Message with id {guid} sent successfully.\n");
    }
}