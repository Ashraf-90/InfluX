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
    [Route("api/InfluencerMedia")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Influencer")]
    public class InfluencerMediaApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IInfluencerMediaServices _influencerMedia;
        private readonly IMapper _mapper;

        public InfluencerMediaApiController(
            AppDBContext appDb,
            IInfluencerMediaServices influencerMedia,
            IMapper mapper)
        {
            _appDb = appDb;
            _influencerMedia = influencerMedia;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/InfluencerMedia/GetInfluencerMedia
        // يرجع كل الميديا الخاصة بالمستخدم (Influencer) الحالي
        // =========================================================
        [HttpGet("GetInfluencerMedia")]
        public async Task<ActionResult<ApiResponse<List<InfluencerMediaDto>>>> GetInfluencerMedia()
        {
            var userId = GetUserId();

            var entities = await _appDb.InfluencerMedia
                .AsNoTracking()
                .Where(x => x.InfluencerId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<InfluencerMediaDto>>(entities);

            return Ok(new ApiResponse<List<InfluencerMediaDto>>
            {
                Success = true,
                Message = "Influencer media loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // GET: api/InfluencerMedia/GetInfluencerMediaById/{id}
        // =========================================================
        [HttpGet("GetInfluencerMediaById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<InfluencerMediaDto>>> GetInfluencerMediaById(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerMedia
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<InfluencerMediaDto>
                {
                    Success = false,
                    Message = "Influencer media not found."
                });
            }

            var dto = _mapper.Map<InfluencerMediaDto>(entity);

            return Ok(new ApiResponse<InfluencerMediaDto>
            {
                Success = true,
                Message = "Influencer media loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/InfluencerMedia/CreateInfluencerMedia
        // Body: InfluencerMediaCreateDto (InfluencerId يتم أخذه من التوكن)
        // =========================================================
        [HttpPost("CreateInfluencerMedia")]
        public async Task<ActionResult<ApiResponse<object>>> CreateInfluencerMedia([FromBody] InfluencerMediaCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var userId = GetUserId();
            req.InfluencerId = userId; // ✅ من التوكن فقط (DTO يحتوي InfluencerId) :contentReference[oaicite:4]{index=4}

            var ok = await _influencerMedia.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create influencer media."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer media created successfully."
            });
        }

        // =========================================================
        // PUT: api/InfluencerMedia/UpdateInfluencerMedia/{id}
        // =========================================================
        [HttpPut("UpdateInfluencerMedia/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateInfluencerMedia(Guid id, [FromBody] InfluencerMediaUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerMedia
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Influencer media not found."
                });
            }

            req.Id = id; // ✅ مهم لأن service يعتمد على Id :contentReference[oaicite:5]{index=5}

            var ok = await _influencerMedia.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update influencer media."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer media updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/InfluencerMedia/DeleteInfluencerMedia/{id}
        // Soft Delete
        // =========================================================
        [HttpDelete("DeleteInfluencerMedia/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteInfluencerMedia(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerMedia
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Influencer media not found."
                });
            }

            var ok = await _influencerMedia.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete influencer media."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer media deleted successfully."
            });
        }
    }
}