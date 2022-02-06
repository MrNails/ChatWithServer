using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Models
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ChatPreview
    {
        public ChatPreview(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}