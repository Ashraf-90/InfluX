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
    [Route("api/InfluencerAssets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Influencer")]
    public class InfluencerAssetApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IInfluencerAssetServices _assets;
        private readonly IMapper _mapper;

        public InfluencerAssetApiController(
            AppDBContext appDb,
            IInfluencerAssetServices assets,
            IMapper mapper)
        {
            _appDb = appDb;
            _assets = assets;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/InfluencerAssets/GetInfluencerAssets
        // List (للـ influencer الحالي)
        // =========================================================
        [HttpGet("GetInfluencerAssets")]
        public async Task<ActionResult<ApiResponse<List<InfluencerAssetDto>>>> GetInfluencerAssets()
        {
            var userId = GetUserId();

            var entities = await _appDb.InfluencerAssets
                .AsNoTracking()
                .Where(x => x.InfluencerId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<InfluencerAssetDto>>(entities);

            return Ok(new ApiResponse<List<InfluencerAssetDto>>()
            {
                Success = true,
                Message = "Influencer assets loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // GET: api/InfluencerAssets/GetInfluencerAssetById/{id}
        // =========================================================
        [HttpGet("GetInfluencerAssetById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<InfluencerAssetDto>>> GetInfluencerAssetById(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerAssets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<InfluencerAssetDto>
                {
                    Success = false,
                    Message = "Influencer asset not found."
                });
            }

            var dto = _mapper.Map<InfluencerAssetDto>(entity);

            return Ok(new ApiResponse<InfluencerAssetDto>
            {
                Success = true,
                Message = "Influencer asset loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/InfluencerAssets/CreateInfluencerAsset
        // Body: InfluencerAssetCreateDto (InfluencerId من التوكن)
        // =========================================================
        [HttpPost("CreateInfluencerAsset")]
        public async Task<ActionResult<ApiResponse<object>>> CreateInfluencerAsset([FromBody] InfluencerAssetCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            req.InfluencerId = GetUserId(); // DTO يحتوي InfluencerId :contentReference[oaicite:4]{index=4}

            var ok = await _assets.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create influencer asset."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer asset created successfully."
            });
        }

        // =========================================================
        // PUT: api/InfluencerAssets/UpdateInfluencerAsset/{id}
        // Body: InfluencerAssetUpdateDto
        // =========================================================
        [HttpPut("UpdateInfluencerAsset/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateInfluencerAsset(Guid id, [FromBody] InfluencerAssetUpdateDto req)
        {
            var userId = GetUserId();

            // Ownership check (asset belongs to current influencer)
            var entity = await _appDb.InfluencerAssets
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Influencer asset not found."
                });
            }

            req.Id = id;                 // DTO يحتوي Id :contentReference[oaicite:5]{index=5}
            req.InfluencerId = userId;   // تثبيت الملكية

            var ok = await _assets.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update influencer asset."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer asset updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/InfluencerAssets/DeleteInfluencerAsset/{id}
        // Soft Delete
        // =========================================================
        [HttpDelete("DeleteInfluencerAsset/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteInfluencerAsset(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerAssets
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Influencer asset not found."
                });
            }

            var ok = await _assets.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete influencer asset."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Influencer asset deleted successfully."
            });
        }
    }
}