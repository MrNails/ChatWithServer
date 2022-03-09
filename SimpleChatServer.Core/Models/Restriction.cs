using System;

namespace SimpleChatServer.Core.Models;

[Flags]
public enum Restriction
{
    None = 0x0000,
    
    /// <summary>
    /// User cannot kick other user
    /// </summary>
    NoKicks = 0x0001,
    
    /// <summary>
    /// User cannot send images
    /// </summary>
    NoImage = 0x0002,
    
    /// <summary>
    /// User cannot adding new admins
    /// </summary>
    NoAddingAdmins = 0x0004,
    
    /// <summary>
    /// User cannot change chat information
    /// </summary>
    NoChangingChatInfo = 0x0008,
}