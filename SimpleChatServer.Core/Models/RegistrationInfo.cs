namespace SimpleChatServer.Core.Models
{
    public readonly struct RegistrationInfo
    {
        public RegistrationInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}