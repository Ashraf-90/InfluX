using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/BrandProfiles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BrandProfileApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IBrandProfileServices _brandProfiles;
        private readonly IMapper _mapper;

        public BrandProfileApiController(AppDBContext appDb, IBrandProfileServices brandProfiles, IMapper mapper)
        {
            _appDb = appDb;
            _brandProfiles = brandProfiles;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/BrandProfiles/GetMyBrandProfile
        // =========================================================
        [HttpGet("GetBrandProfile")]
        public async Task<ActionResult<ApiResponse<BrandProfileDto>>> GetMyBrandProfile()
        {
            var userId = GetUserId();

            var entity = await _appDb.BrandProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<BrandProfileDto> { Success = false, Message = "Brand profile not found." });
            }

            var dto = _mapper.Map<BrandProfileDto>(entity);

            return Ok(new ApiResponse<BrandProfileDto>
            {
                Success = true,
                Message = "Brand profile loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/BrandProfiles/CreateBrandProfile
        // =========================================================
        [HttpPost("CreateBrandProfile")]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrandProfile([FromBody] BrandProfileCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid request body." });
            }

            var userId = GetUserId();

            // ابحث عن أي BrandProfile للمستخدم حتى لو SoftDeleted
            var existing = await _appDb.BrandProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            // إذا موجود وفعّال => لا يمكن إنشاء واحد جديد (One-to-One)
            if (existing != null && existing.Active)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Brand profile already exists for this user."
                });
            }

            // إذا موجود لكنه SoftDeleted => Restore + Update
            if (existing != null && !existing.Active)
            {
                existing.Active = true;
                existing.BrandName = req.BrandName;
                existing.Website = req.Website;
                existing.Description = req.Description;
                existing.LogoUrl = req.LogoUrl;
                existing.Industry = req.Industry;
                existing.Country = req.Country;
                existing.City = req.City;

                await _appDb.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Brand profile restored successfully."
                });
            }

            // غير موجود إطلاقاً => Create جديد
            req.UserId = userId;

            var ok = await _brandProfiles.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create brand profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Brand profile created successfully." });
        }

        // =========================================================
        // PUT: api/BrandProfiles/UpdateBrandProfile/{id}
        // =========================================================
        [HttpPut("UpdateBrandProfile/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBrandProfile(Guid id, [FromBody] BrandProfileUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.BrandProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Brand profile not found." });
            }

            req.Id = id;
            req.UserId = userId;

            var ok = await _brandProfiles.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update brand profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Brand profile updated successfully." });
        }

        // =========================================================
        // DELETE: api/BrandProfiles/DeleteBrandProfile/{id}
        // Soft Delete
        // =========================================================
        [HttpDelete("DeleteBrandProfile/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBrandProfile(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.BrandProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Brand profile not found." });
            }

            var ok = await _brandProfiles.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete brand profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Brand profile deleted successfully." });
        }
    }
}