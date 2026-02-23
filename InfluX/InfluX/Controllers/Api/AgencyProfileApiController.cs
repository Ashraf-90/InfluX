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
    [Route("api/AgencyProfiles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AgencyProfileApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IAgencyProfileServices _agencyProfiles;
        private readonly IMapper _mapper;

        public AgencyProfileApiController(AppDBContext appDb, IAgencyProfileServices agencyProfiles, IMapper mapper)
        {
            _appDb = appDb;
            _agencyProfiles = agencyProfiles;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        [HttpGet("GetAgencyProfile")]
        public async Task<ActionResult<ApiResponse<AgencyProfileDto>>> GetMyAgencyProfile()
        {
            var userId = GetUserId();

            var entity = await _appDb.AgencyProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<AgencyProfileDto> { Success = false, Message = "Agency profile not found." });
            }

            var dto = _mapper.Map<AgencyProfileDto>(entity);

            return Ok(new ApiResponse<AgencyProfileDto>
            {
                Success = true,
                Message = "Agency profile loaded successfully.",
                Data = dto
            });
        }

        [HttpPost("CreateAgencyProfile")]
        public async Task<ActionResult<ApiResponse<object>>> CreateAgencyProfile([FromBody] AgencyProfileCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid request body." });
            }

            var userId = GetUserId();

            // ابحث عن أي AgencyProfile للمستخدم حتى لو SoftDeleted
            var existing = await _appDb.AgencyProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            // إذا موجود وفعّال => لا يمكن إنشاء واحد جديد (One-to-One)
            if (existing != null && existing.Active)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Agency profile already exists for this user."
                });
            }

            // إذا موجود لكنه SoftDeleted => Restore + Update
            if (existing != null && !existing.Active)
            {
                existing.Active = true;
                existing.AgencyName = req.AgencyName;
                existing.Website = req.Website;
                existing.Description = req.Description;
                existing.LogoUrl = req.LogoUrl;

                await _appDb.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Agency profile restored successfully."
                });
            }

            // غير موجود إطلاقاً => Create جديد
            req.UserId = userId;

            var ok = await _agencyProfiles.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create agency profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency profile created successfully." });
        }

        [HttpPut("UpdateAgencyProfile/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAgencyProfile(Guid id, [FromBody] AgencyProfileUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.AgencyProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Agency profile not found." });
            }

            req.Id = id;
            req.UserId = userId;

            var ok = await _agencyProfiles.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update agency profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency profile updated successfully." });
        }

        [HttpDelete("DeleteAgencyProfile/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAgencyProfile(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.AgencyProfiles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Agency profile not found." });
            }

            var ok = await _agencyProfiles.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete agency profile." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency profile deleted successfully." });
        }
    }
}