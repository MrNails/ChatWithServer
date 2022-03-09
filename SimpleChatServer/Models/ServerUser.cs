using System.Net.Sockets;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.Models
{
    internal class ServerUser
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