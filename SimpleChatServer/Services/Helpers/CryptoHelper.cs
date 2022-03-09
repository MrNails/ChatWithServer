using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using SimpleChatServer.DAL.Models;

namespace SimpleChatServer.Services.Helpers;

public static class CryptoHelper
{
    private static readonly SHA256 s_sha256Encryptor = SHA256.Create();
    private static readonly JwtSecurityTokenHandler s_jwtHandler = new JwtSecurityTokenHandler();
    
    public static byte[] EncryptData(byte[] data, byte[] salt)
    {
        if (data.Length == 0 || salt.Length == 0)
            return Array.Empty<byte>();
        
        var encryptedStr = new byte[data.Length + salt.Length];

        for (int i = 0; i < data.Length; i++)
            encryptedStr[i] = data[i];

        for (int i = 0; i < salt.Length; i++)
            encryptedStr[data.Length + i] = salt[i];

        return s_sha256Encryptor.ComputeHash(encryptedStr);
    }

    public static string CreateJWT(User user)
    {
        var utcNow = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
            GlobalSettings.JwtSettings.Issuer,
            GlobalSettings.JwtSettings.Audience,
            CreateClaims(user),
            utcNow,
            utcNow.Add(TimeSpan.FromMinutes(GlobalSettings.AuthOptions.Lifetime)),
            new SigningCredentials(GlobalSettings.JwtSettings.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );
        
        return s_jwtHandler.WriteToken(jwt);
    }

    private static ReadOnlyCollection<Claim> CreateClaims(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim("Login", user.Login)
        };

        return claims.AsReadOnly();
    }
}