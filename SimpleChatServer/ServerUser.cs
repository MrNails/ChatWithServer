using System.Net.Sockets;
using SimpleChatServer.Core.Models;

namespace SimpleChatServer
{
    public readonly struct ServerUser
    {
        private readonly TcpClient m_userClient;
        private readonly User m_userInfo;

        public ServerUser(TcpClient userClient, User userInfo)
        {
            m_userClient = userClient;
            m_userInfo = userInfo;
        }

        public TcpClient UserClient => m_userClient;
        public User UserInfo => m_userInfo;
    }
}