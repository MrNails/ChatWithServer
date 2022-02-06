namespace SimpleChatServer.Core.Services
{
    public interface ISender
    {
        long Id { get; }
        string Name { get; set; }
    }
}