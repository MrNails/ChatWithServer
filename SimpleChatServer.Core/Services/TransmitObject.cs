using System;
using System.Runtime.InteropServices;

namespace SimpleChatServer.Core.Services
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct TransmitObject
    {
        public TransmitObject(string transmitType, byte[] content, int contentSize, Guid id)
        {
            TransmitType = transmitType;
            Content = content;
            Id = id;

            if (contentSize < 0)
                throw new ArgumentException("Size cannot be less than 0.");
            else
                ContentSize = contentSize;
        }

        public int ContentSize { get; }
        public Guid Id { get; }
        public string TransmitType { get; }
        public byte[] Content { get; }
    }
}