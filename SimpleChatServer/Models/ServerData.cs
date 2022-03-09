using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using SimpleChatServer.Core.Services.Handlers;
using SimpleChatServer.DataStructures;
using SimpleChatServer.Services.Handlers;

namespace SimpleChatServer.Models;

internal class ServerData
{
    private readonly HashSet<string> m_sessions;
    private readonly Dictionary<string, IRequestHandler> m_requestHandlers;
    // private readonly Dictionary<TcpClient, Request> m_requests;

    public ServerData()
    {
        m_sessions = new HashSet<string>();
        m_requestHandlers = new Dictionary<string, IRequestHandler>();
        // m_requests = new Dictionary<TcpClient, Request>();

    }

    public HashSet<string> Sessions => m_sessions;
    public Dictionary<string, IRequestHandler> Handlers => m_requestHandlers;

    // /// <summary>
    // /// Using instead of dependency injection as fastest way to simulate it.
    // /// Represent Request data for specified client
    // /// </summary>
    // public Dictionary<TcpClient, Request> Requests => m_requests;
}