using System.ComponentModel.DataAnnotations;

namespace InfluX.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "أدخل بريداً إلكترونياً صحيحاً")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم المستخدم مطلوب")]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "اسم المستخدم")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "اسم العرض مطلوب")]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "اسم العرض")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "تأكيد كلمة المرور")]
    [Compare("Password", ErrorMessage = "كلمة المرور والتأكيد غير متطابقتين")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
