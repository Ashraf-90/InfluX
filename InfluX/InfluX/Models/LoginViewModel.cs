using System.ComponentModel.DataAnnotations;

namespace InfluX.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "اسم المستخدم أو البريد مطلوب")]
    [Display(Name = "اسم المستخدم / البريد")]
    public string UserNameOrEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    [Display(Name = "كلمة المرور")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "تذكرني")]
    public bool RememberMe { get; set; }
}
