namespace SimpleChatServer.Core.Models
{
    public readonly struct UserCerdentials
    {
        public UserCerdentials(string login, byte[] password, bool isForRegistration)
        {
            Login = login;
            Password = password;
            IsForRegistration = isForRegistration;
        }

        public bool IsForRegistration { get; }
        
        public string Login { get; }
        public byte[] Password { get; }
    }
}