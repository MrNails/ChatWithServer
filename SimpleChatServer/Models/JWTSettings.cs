using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SimpleChatServer.Models;

internal class JWTSettings
{
    public JWTSettings(string issuer, string audience, string key)
    {
        Issuer = issuer;
        Audience = audience;
        Key = key;

        SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
    
    public string Issuer { get; }
    public string Audience { get; }
    public string Key { get; }

    public SymmetricSecurityKey SymmetricSecurityKey { get; }
}