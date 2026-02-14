using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using InfluX.Models;
using Microsoft.EntityFrameworkCore;

using AppUser = Domain.Entities.ApplicationUser;
using Domain.Entities;

namespace InfluX.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly AppDBContext _appDb;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            AppDBContext appDb)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _appDb = appDb;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var existsByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existsByEmail != null)
            {
                ModelState.AddModelError(string.Empty, "البريد الإلكتروني مستخدم مسبقاً.");
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                var existsByUserName = await _userManager.FindByNameAsync(model.UserName);
                if (existsByUserName != null)
                {
                    ModelState.AddModelError(string.Empty, "اسم المستخدم مستخدم مسبقاً.");
                    return View(model);
                }
            }

            var user = new AppUser
            {
                UserName = string.IsNullOrWhiteSpace(model.UserName) ? model.Email : model.UserName,
                Email = model.Email,

                AppRole = "User",
                Active = true,
                IsAvilable = true,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            // optional role assignment (لا نكسر لو فشل)
            try { await _userManager.AddToRoleAsync(user, "User"); } catch { }

            // Create UserProfile (App DB)
            var profile = await _appDb.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = user.Id,
                    FullName = model.DisplayName,

                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _appDb.UserProfiles.Add(profile);
                await _appDb.SaveChangesAsync(cancellationToken);
            }

            _logger.LogInformation("تم إنشاء حساب جديد: {UserName}", user.UserName);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null, CancellationToken cancellationToken = default)
        {
            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var input = model.UserNameOrEmail?.Trim() ?? "";

            var user = input.Contains('@')
                ? await _userManager.FindByEmailAsync(input)
                : await _userManager.FindByNameAsync(input);

            if (user == null || !user.Active)
            {
                ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("تم تسجيل الدخول: {UserName}", user.UserName);
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("تم تسجيل الخروج");

            returnUrl ??= Url.Content("~/");
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
