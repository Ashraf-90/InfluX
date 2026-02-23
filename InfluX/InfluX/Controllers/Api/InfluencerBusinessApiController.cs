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
    [Route("api/InfluencerBusinesses")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InfluencerBusinessApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IInfluencerBusinessServices _business;
        private readonly IMapper _mapper;

        public InfluencerBusinessApiController(AppDBContext appDb, IInfluencerBusinessServices business, IMapper mapper)
        {
            _appDb = appDb;
            _business = business;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/InfluencerBusinesses/GetMyInfluencerBusinesses
        // =========================================================
        [HttpGet("GetMyInfluencerBusinesses")]
        public async Task<ActionResult<ApiResponse<List<InfluencerBusinessDto>>>> GetMyInfluencerBusinesses()
        {
            var userId = GetUserId();

            var entities = await _appDb.InfluencerBusinesses
                .AsNoTracking()
                .Where(x => x.InfluencerId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<InfluencerBusinessDto>>(entities);

            return Ok(new ApiResponse<List<InfluencerBusinessDto>>
            {
                Success = true,
                Message = "Influencer businesses loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/InfluencerBusinesses/CreateInfluencerBusiness
        // =========================================================
        [HttpPost("CreateInfluencerBusiness")]
        public async Task<ActionResult<ApiResponse<object>>> CreateInfluencerBusiness([FromBody] InfluencerBusinessCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid request body." });
            }

            var userId = GetUserId();
            req.InfluencerId = userId;

            var ok = await _business.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create influencer business." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Influencer business created successfully." });
        }

        // =========================================================
        // PUT: api/InfluencerBusinesses/UpdateInfluencerBusiness/{id}
        // =========================================================
        [HttpPut("UpdateInfluencerBusiness/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateInfluencerBusiness(Guid id, [FromBody] InfluencerBusinessUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerBusinesses
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Influencer business not found." });
            }

            req.Id = id;
            req.InfluencerId = userId;

            var ok = await _business.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update influencer business." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Influencer business updated successfully." });
        }

        // =========================================================
        // DELETE: api/InfluencerBusinesses/DeleteInfluencerBusiness/{id}
        // Soft Delete
        // =========================================================
        [HttpDelete("DeleteInfluencerBusiness/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteInfluencerBusiness(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.InfluencerBusinesses
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Influencer business not found." });
            }

            var ok = await _business.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete influencer business." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Influencer business deleted successfully." });
        }
    }
}