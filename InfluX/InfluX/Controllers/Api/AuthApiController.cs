using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDBContext _appDb;
        private readonly IJwtTokenService _jwt;

        public AuthApiController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDBContext appDb,
            IJwtTokenService jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDb = appDb;
            _jwt = jwt;
        }

        // =========================
        // POST: /api/auth/register
        // =========================
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<MobileUserDto>>> Register([FromBody] RegisterRequestDto req)
        {
            var exists = await _userManager.FindByEmailAsync(req.Email);
            if (exists != null)
            {
                return BadRequest(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = "Email already registered."
                });
            }

            var user = new ApplicationUser
            {
                UserName = req.Email,
                Email = req.Email,
                PhoneNumber = req.PhoneNumber,
                AppRole = req.Role ?? "Client",
                Active = true,
                IsAvilable = true,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            var createRes = await _userManager.CreateAsync(user, req.Password);
            if (!createRes.Succeeded)
            {
                return BadRequest(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = string.Join(" | ", createRes.Errors.Select(e => e.Description))
                });
            }

            // Role (optional) - only if role exists in Identity DB
            var roleName = req.Role ?? "Client";
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                // If role doesn't exist, AddToRoleAsync will fail -> ignore gracefully
                var addRoleRes = await _userManager.AddToRoleAsync(user, roleName);
                // no hard fail to avoid breaking old setup
            }

            // Create/Update UserProfile in AppDBContext
            var profile = await _appDb.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = user.Id,
                    FullName = req.FullName,
                    Country = req.Country,
                    City = req.City,
                    Language = req.Language,
                    AvatarUrl = req.AvatarUrl,

                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };
                _appDb.UserProfiles.Add(profile);
            }
            else
            {
                profile.FullName = req.FullName ?? profile.FullName;
                profile.Country = req.Country ?? profile.Country;
                profile.City = req.City ?? profile.City;
                profile.Language = req.Language ?? profile.Language;
                profile.AvatarUrl = req.AvatarUrl ?? profile.AvatarUrl;
                profile.UpdateDate = DateTime.UtcNow;
            }

            await _appDb.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwt.GenerateToken(user, roles);

            var dto = new MobileUserDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault(),
                AppRole = user.AppRole,

                FullName = profile.FullName,
                Country = profile.Country,
                City = profile.City,
                Language = profile.Language,
                AvatarUrl = profile.AvatarUrl,
                CreateDate = user.CreateDate
            };

            return Ok(new ApiResponse<MobileUserDto>
            {
                Success = true,
                Message = "Registration completed successfully.",
                Data = dto,
                Token = $"Bearer {token}"
            });
        }

        // =========================
        // POST: /api/auth/login
        // =========================
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<MobileUserDto>>> Login([FromBody] LoginRequestDto req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // Check password
            var passOk = await _userManager.CheckPasswordAsync(user, req.Password);
            if (!passOk)
            {
                return Unauthorized(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }

            // Load profile
            var profile = await _appDb.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwt.GenerateToken(user, roles);

            var dto = new MobileUserDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault(),
                AppRole = user.AppRole,

                FullName = profile?.FullName,
                Country = profile?.Country,
                City = profile?.City,
                Language = profile?.Language,
                AvatarUrl = profile?.AvatarUrl,
                CreateDate = user.CreateDate
            };

            return Ok(new ApiResponse<MobileUserDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = dto,
                Token = token
            });
        }

        // =========================
        // POST: /api/auth/forget-password
        // NOTE: returns reset token (for mobile dev). In production send email/SMS.
        // =========================
        [HttpPost("forget-password")]
        public async Task<ActionResult<ApiResponse<object>>> ForgetPassword([FromBody] ForgetPasswordRequestDto req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                // Don't reveal if email exists
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "If the email exists, a reset token has been generated."
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Reset token generated successfully.",
                Data = new { email = req.Email, token }
            });
        }

        // =========================
        // POST: /api/auth/reset-password
        // =========================
        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordRequestDto req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request."
                });
            }

            var res = await _userManager.ResetPasswordAsync(user, req.Token, req.NewPassword);
            if (!res.Succeeded)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = string.Join(" | ", res.Errors.Select(e => e.Description))
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password reset successfully."
            });
        }

        // =========================
        // POST: /api/auth/logout
        // JWT logout is client-side (delete token), but we keep endpoint for mobile flow.
        // =========================
        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<object>>> Logout()
        {
            // Does not invalidate JWT (no blacklist yet). Cookie signout won't hurt web.
            await _signInManager.SignOutAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Logout successful."
            });
        }
    }
}
