﻿namespace SimpleChatServer.DAL.Models;

public class UserProfile
{
    public int UserId { get; set; }
    
    public string? Name { get; set; }
    public string? Bio { get; set; }
}