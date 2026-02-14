using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/InfluencerProfile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InfluencerProfileApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IInfluencerProfileServices _influencerProfileServices;

        public InfluencerProfileApiController( AppDBContext appDb, IInfluencerProfileServices influencerProfileServices)
        {
            _appDb = appDb;
            _influencerProfileServices = influencerProfileServices;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: /api/influencer-profile/get
        // =========================================================
        [HttpGet("GetInfluencerProfile")]
        public async Task<ActionResult<ApiResponse<InfluencerProfileDto>>> GetProfile()
        {
            var userId = GetUserId();

            // Global filter Active=true يطبق تلقائياً
            var entity = await _appDb.InfluencerProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<InfluencerProfileDto>
                {
                    Success = false,
                    Message = "Influencer profile not found."
                });
            }

            var dto = await _influencerProfileServices.GetById(entity.Id);

            return Ok(new ApiResponse<InfluencerProfileDto>
            {
                Success = true,
                Message = "Influencer profile loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: /api/influencer-profile/create
        // - UserId من التوكن فقط
        // - إذا كان موجود Active=true => Conflict
        // - إذا كان SoftDeleted Active=false => Restore
        // =========================================================
        [HttpPost("CreateInfluencerProfile")]
        public async Task<ActionResult<ApiResponse<InfluencerProfileDto>>> CreateProfile([FromBody] InfluencerProfileCreateDto req)
        {
            var userId = GetUserId();

            // Include soft-deleted rows
            var existing = await _appDb.InfluencerProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (existing != null && existing.Active)
            {
                return Conflict(new ApiResponse<InfluencerProfileDto>
                {
                    Success = false,
                    Message = "Influencer profile already exists."
                });
            }

            // Restore if soft deleted
            if (existing != null && !existing.Active)
            {
                existing.Active = true;
                existing.IsAvilable = true;

                existing.Username = req.Username;
                existing.Bio = req.Bio;
                existing.Gender = req.Gender;
                existing.Birthdate = req.Birthdate;
                existing.PublicSlug = req.PublicSlug;

                // لا نسمح للموبايل بتغيير IsVerified
                // existing.IsVerified يبقى كما هو

                await _appDb.SaveChangesAsync();

                var restoredDto = await _influencerProfileServices.GetById(existing.Id);

                return Ok(new ApiResponse<InfluencerProfileDto>
                {
                    Success = true,
                    Message = "Influencer profile restored successfully.",
                    Data = restoredDto
                });
            }

            // Create new
            var createDto = new InfluencerProfileCreateDto
            {
                UserId = userId, // ✅ من التوكن فقط

                Username = req.Username,
                Bio = req.Bio,
                Gender = req.Gender,
                Birthdate = req.Birthdate,
                PublicSlug = req.PublicSlug,

                Active = true,
                IsAvilable = true
            };

            var ok = await _influencerProfileServices.Create(createDto);
            if (!ok)
            {
                return BadRequest(new ApiResponse<InfluencerProfileDto>
                {
                    Success = false,
                    Message = "Failed to create influencer profile."
                });
            }

            var created = await _appDb.InfluencerProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var dto = created == null ? null : await _influencerProfileServices.GetById(created.Id);

            return Ok(new ApiResponse<InfluencerProfileDto>
            {
                Success = true,
                Message = "Influencer profile created successfully.",
                Data = dto
            });
        }

        // =========================================================
        // PUT: /api/influencer-profile/update
        // - لا نأخذ Id من الموبايل
        // - نحدد السجل من userId (التوكن)
        // - لا نسمح بتغيير IsVerified من الموبايل
        // =========================================================
        [HttpPut("UpdateInfluencerProfile")]
        public async Task<ActionResult<ApiResponse<InfluencerProfileDto>>> UpdateProfile([FromBody] InfluencerProfileUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<InfluencerProfileDto>
                {
                    Success = false,
                    Message = "Influencer profile not found."
                });
            }

            var updateDto = new InfluencerProfileUpdateDto
            {
                Id = entity.Id, // ✅ من DB وليس من الموبايل

                Username = req.Username,
                Bio = req.Bio,
                Gender = req.Gender,
                Birthdate = req.Birthdate,
                PublicSlug = req.PublicSlug,

                IsVerified = entity.IsVerified, // ✅ ممنوع تغييره من الموبايل

                Active = entity.Active,
                IsAvilable = entity.IsAvilable
            };

            var ok = await _influencerProfileServices.Update(updateDto);
            if (!ok)
            {
                return BadRequest(new ApiResponse<InfluencerProfileDto>
                {
                    Success = false,
                    Message = "Failed to update influencer profile."
                });
            }

            var dto = await _influencerProfileServices.GetById(entity.Id);

            return Ok(new ApiResponse<InfluencerProfileDto>
            {
                Success = true,
                Message = "Influencer profile updated successfully.",
                Data = dto
            });
        }

        // =========================================================
        // DELETE: /api/influencer-profile/delete
        // Soft Delete => Active=false
        // =========================================================
        [HttpDelete("DeleteInfluencerProfile")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteProfile()
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Influencer profile not found."
                });
            }

            var ok = await _influencerProfileServices.SoftDelete(entity.Id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete influencer profile."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer profile deleted successfully."
            });
        }
    }
}
