using Microsoft.AspNetCore.Identity;

namespace InfluX.Models;

/// <summary>
/// نموذج المستخدم المخصص - يمتد IdentityUser
/// Custom Application User with extra properties for chat
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// اسم العرض في الشات
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// رابط صورة البروفايل (اختياري)
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// تاريخ إنشاء الحساب
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// آخر ظهور (للشات)
    /// </summary>
    public DateTime? LastSeenAt { get; set; }
}
