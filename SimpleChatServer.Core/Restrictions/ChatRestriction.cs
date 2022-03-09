namespace SimpleChatServer.Core.Restrictions;

public class ChatRestriction
{
    public ChatRestriction(int maxNameLength, int minNameLength, int maxUsers)
    {
        MaxNameLength = maxNameLength;
        MinNameLength = minNameLength;
        MaxUsers = maxUsers;
    }

    public int MaxNameLength { get; }
    public int MinNameLength { get; }
    public int MaxUsers { get; }
}