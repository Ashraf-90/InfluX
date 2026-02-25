using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/AgencyClients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
    public class AgencyClientsApiController : ControllerBase
    {
        private readonly AppDBContext _db;
        private readonly IAgencyClientServices _svc;

        public AgencyClientsApiController(AppDBContext db, IAgencyClientServices svc)
        {
            _db = db;
            _svc = svc;
        }

        // GET: api/AgencyClients/GetAll
        [HttpGet("GetAllAgencyClients")]
        public async Task<ActionResult<ApiResponse<object>>> GetAll()
        {
            var list = await _db.AgencyClients
                .AsNoTracking()
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new
                {
                    id = x.Id,
                    agencyProfileId = x.AgencyProfileId,
                    brandProfileId = x.BrandProfileId,
                    role = x.Role,
                    status = x.Status
                })
                .ToListAsync();

            return Ok(new ApiResponse<object> { Success = true, Message = "Loaded.", Data = list });
        }

        // POST: api/AgencyClients/Create
        [HttpPost("CreateAgencyClient")]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] AgencyClientCreateDto req)
        {
            if (req == null || req.AgencyProfileId == Guid.Empty || req.BrandProfileId == Guid.Empty)
                return BadRequest(new ApiResponse<object> { Success = false, Message = "agencyProfileId & brandProfileId are required." });

            // تأكد AgencyProfile موجود
            var agencyExists = await _db.AgencyProfiles
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(x => x.Id == req.AgencyProfileId);

            if (!agencyExists)
                return BadRequest(new ApiResponse<object> { Success = false, Message = $"Invalid agencyProfileId: {req.AgencyProfileId}" });

            // تأكد BrandProfile موجود
            var brandExists = await _db.BrandProfiles
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(x => x.Id == req.BrandProfileId);

            if (!brandExists)
                return BadRequest(new ApiResponse<object> { Success = false, Message = $"Invalid brandProfileId: {req.BrandProfileId}" });

            // منع duplicate (Active فقط)
            var already = await _db.AgencyClients
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(x => x.AgencyProfileId == req.AgencyProfileId && x.BrandProfileId == req.BrandProfileId && x.Active);

            if (already)
                return Conflict(new ApiResponse<object> { Success = false, Message = "This agency is already linked to this brand." });

            var ok = await _svc.Create(req);
            if (!ok)
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create." });

            return Ok(new ApiResponse<object> { Success = true, Message = "Created successfully." });
        }

        // PUT: api/AgencyClients/Update/{id}
        [HttpPut("UpdateAgencyClient/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(Guid id, [FromBody] AgencyClientUpdateDto req)
        {
            if (req == null) return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid body." });

            req.Id = id;
            var ok = await _svc.Update(req);

            if (!ok)
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update." });

            return Ok(new ApiResponse<object> { Success = true, Message = "Updated successfully." });
        }

        // DELETE: api/AgencyClients/Delete/{id}
        [HttpDelete("DeleteAgencyClient/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
        {
            var ok = await _svc.SoftDelete(id);
            if (!ok)
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete." });

            return Ok(new ApiResponse<object> { Success = true, Message = "Deleted successfully." });
        }
    }
}