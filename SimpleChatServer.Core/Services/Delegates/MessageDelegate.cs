using SimpleChatServer.Core.Models;

namespace SimpleChatServer.Core.Services.Delegates
{
    public delegate void MessageDelegate(Chat sender, Message message);
}