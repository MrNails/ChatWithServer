using System;
using System.Collections.Generic;
using SimpleChatServer.Models;

namespace SimpleChatServer.Services;

internal static class GlobalSettings
{
    public static readonly Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();
    private static JWTSettings _jwtSettings;
    private static AuthenticationOptions _authOptions;

    public static JWTSettings JwtSettings
    {
        get => _jwtSettings;
        set => _jwtSettings = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static AuthenticationOptions AuthOptions
    {
        get => _authOptions;
        set => _authOptions = value ?? throw new ArgumentNullException(nameof(value));
    }
}