using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/AgencyClients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
    public class AgencyClientsApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IAgencyClientServices _agencyClients;
        private readonly IMapper _mapper;

        public AgencyClientsApiController(AppDBContext appDb, IAgencyClientServices agencyClients, IMapper mapper)
        {
            _appDb = appDb;
            _agencyClients = agencyClients;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/AgencyClients/GetMyAgencyClients
        // =========================================================
        [HttpGet("GetAgencyClients")]
        public async Task<ActionResult<ApiResponse<object>>> GetMyAgencyClients()
        {
            var agencyId = GetUserId();

            var list = await _appDb.AgencyClients
                .AsNoTracking()
                .Where(x => x.AgencyId == agencyId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new
                {
                    id = x.Id,
                    brandId = x.BrandId,
                    role = x.Role,
                    status = x.Status
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Agency clients loaded successfully.",
                Data = list
            });
        }

        // =========================================================
        // POST: api/AgencyClients/CreateAgencyClient
        // Body: { "brandId": "GUID", "role": 1|2, "status": 1|2 }
        // =========================================================
        [HttpPost("CreateAgencyClient")]
        public async Task<ActionResult<ApiResponse<object>>> CreateAgencyClient([FromBody] AgencyClientCreateDto req)
        {
            var agencyId = GetUserId();
            req.AgencyId = agencyId;

            if (req == null || req.BrandId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "brandId is required." });
            }

            // تأكد أن البراند موجود
            var brandExists = await _appDb.Set<ApplicationUser>()
                .AsNoTracking()
                .AnyAsync(u => u.Id == req.BrandId);

            if (!brandExists)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = $"Invalid brandId: {req.BrandId}" });
            }

            // منع التكرار (مع مراعاة SoftDelete: نبحث في IgnoreQueryFilters)
            var already = await _appDb.AgencyClients
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(x => x.AgencyId == agencyId && x.BrandId == req.BrandId && x.Active);

            if (already)
            {
                return Conflict(new ApiResponse<object> { Success = false, Message = "This brand is already linked to your agency." });
            }

            var ok = await _agencyClients.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create agency client." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency client created successfully." });
        }

        // =========================================================
        // PUT: api/AgencyClients/UpdateAgencyClient/{id}
        // =========================================================
        [HttpPut("UpdateAgencyClient/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateAgencyClient(Guid id, [FromBody] AgencyClientUpdateDto req)
        {
            var agencyId = GetUserId();

            var entity = await _appDb.AgencyClients
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.AgencyId == agencyId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Agency client not found." });
            }

            // تأكد brandId موجود
            if (req == null || req.BrandId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "brandId is required." });
            }

            var brandExists = await _appDb.Set<ApplicationUser>()
                .AsNoTracking()
                .AnyAsync(u => u.Id == req.BrandId);

            if (!brandExists)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = $"Invalid brandId: {req.BrandId}" });
            }

            // منع التكرار لو تغير brandId
            var duplicate = await _appDb.AgencyClients
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(x => x.AgencyId == agencyId && x.BrandId == req.BrandId && x.Id != id && x.Active);

            if (duplicate)
            {
                return Conflict(new ApiResponse<object> { Success = false, Message = "This brand is already linked to your agency." });
            }

            req.Id = id;
            req.AgencyId = agencyId;

            var ok = await _agencyClients.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update agency client." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency client updated successfully." });
        }

        // =========================================================
        // DELETE: api/AgencyClients/DeleteAgencyClient/{id}
        // Soft Delete
        // =========================================================
        [HttpDelete("DeleteAgencyClient/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAgencyClient(Guid id)
        {
            var agencyId = GetUserId();

            var entity = await _appDb.AgencyClients
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.AgencyId == agencyId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Agency client not found." });
            }

            var ok = await _agencyClients.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete agency client." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Agency client deleted successfully." });
        }
    }
}