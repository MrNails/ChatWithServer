namespace SimpleChatServer.Models;

internal class AuthenticationOptions
{
    public AuthenticationOptions(int lifetime)
    {
        Lifetime = lifetime;
    }

    /// <summary>
    /// Represent session lifetime in minutes
    /// </summary>
    public int Lifetime { get; }
}