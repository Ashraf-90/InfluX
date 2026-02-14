using Application.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDBContext _appDb;

        public UserApiController(UserManager<ApplicationUser> userManager, AppDBContext appDb)
        {
            _userManager = userManager;
            _appDb = appDb;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================
        // GET: /api/user/me
        // =========================
        [HttpGet("UserProfile")]
        public async Task<ActionResult<ApiResponse<MobileUserDto>>> Me()
        {
            var userId = GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = "Unauthorized."
                });
            }

            var profile = await _appDb.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
            var roles = await _userManager.GetRolesAsync(user);

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
                Message = "User loaded successfully.",
                Data = dto
            });
        }

        // =========================
        // PUT: /api/user/account  (Update User + UserProfile)
        // =========================
        [HttpPut("ManageAccount")]
        public async Task<ActionResult<ApiResponse<MobileUserDto>>> UpdateAccount([FromBody] UpdateAccountRequestDto req)
        {
            var userId = GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = "Unauthorized."
                });
            }

            // Update Identity user fields
            if (!string.IsNullOrWhiteSpace(req.PhoneNumber))
                user.PhoneNumber = req.PhoneNumber;

            user.UpdateDate = DateTime.UtcNow;

            var userUpdateRes = await _userManager.UpdateAsync(user);
            if (!userUpdateRes.Succeeded)
            {
                return BadRequest(new ApiResponse<MobileUserDto>
                {
                    Success = false,
                    Message = string.Join(" | ", userUpdateRes.Errors.Select(e => e.Description))
                });
            }

            // Update/Create profile
            var profile = await _appDb.UserProfiles.FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = user.Id,
                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };
                _appDb.UserProfiles.Add(profile);
            }

            profile.FullName = req.FullName ?? profile.FullName;
            profile.Country = req.Country ?? profile.Country;
            profile.City = req.City ?? profile.City;
            profile.Language = req.Language ?? profile.Language;
            profile.AvatarUrl = req.AvatarUrl ?? profile.AvatarUrl;
            profile.UpdateDate = DateTime.UtcNow;

            await _appDb.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

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
                Message = "Account updated successfully.",
                Data = dto
            });
        }

        // =========================
        // POST: /api/user/change-password
        // =========================
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordRequestDto req)
        {
            var userId = GetUserId();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Unauthorized."
                });
            }

            var res = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
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
                Message = "Password changed successfully."
            });
        }
    }
}
