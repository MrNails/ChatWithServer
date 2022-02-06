namespace SimpleChatServer.Core.Models
{
    public readonly struct RegistrationInfoResponse
    {
        public RegistrationInfoResponse(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}