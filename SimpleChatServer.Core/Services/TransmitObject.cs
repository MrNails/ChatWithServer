using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Services
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct TransmitObject
    {
        public TransmitObject(string transmitType, byte[] content, int contentSize, Guid id) : 
            this(transmitType, content, contentSize, id, string.Empty)
        { }
        
        public TransmitObject(string transmitType, byte[] content, int contentSize, Guid id, string sessionId)
        {
            TransmitType = transmitType;
            Content = content;
            Id = id;

            if (contentSize < 0)
                throw new ArgumentException("Size cannot be less than 0.");
            else
                ContentSize = contentSize;

            SessionId = sessionId ?? string.Empty;
        }

        public int ContentSize { get; }
        public Guid Id { get; }
        public string TransmitType { get; }
        public string SessionId { get; }
        public byte[] Content { get; }
    }
}