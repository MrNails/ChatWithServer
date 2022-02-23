namespace SimpleChatServer.Core.Models
{
    public readonly struct RegistrationInfo
    {
        public RegistrationInfo(string name, string bio)
        {
            Name = name;
            BIO = bio;
        }

        public string Name { get; }
        public string BIO { get; }
    }
}