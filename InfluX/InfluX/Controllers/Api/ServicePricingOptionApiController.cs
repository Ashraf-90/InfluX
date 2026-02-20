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
    [Route("api/ServicePricingOptions")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServicePricingOptionApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IServicePricingOptionServices _pricingOptions;
        private readonly IMapper _mapper;

        public ServicePricingOptionApiController(AppDBContext appDb,IServicePricingOptionServices pricingOptions,IMapper mapper)
        {
            _appDb = appDb;
            _pricingOptions = pricingOptions;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // PUT: api/ServicePricingOptions/UpdateServicePricingOption/{id}
        // Body: ServicePricingOptionUpdateDto
        // =========================================================
        [HttpPut("UpdateServicePricingOption/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateServicePricingOption(Guid id, [FromBody] ServicePricingOptionUpdateDto req)
        {
            var userId = GetUserId();

            // Ownership check (option -> listing -> influencer)
            var option = await _appDb.ServicePricingOptions
                .IgnoreQueryFilters()
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == id && x.ServiceListing.InfluencerId == userId);

            if (option == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Service pricing option not found."
                });
            }

            // Force ids to prevent moving option to another listing
            req.Id = id;
            req.ServiceListingId = option.ServiceListingId;

            var ok = await _pricingOptions.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update service pricing option."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service pricing option updated successfully."
            });
        }

        // =========================================================
        // GET: api/ServicePricingOptions/GetServicePricingOptions/{serviceId}
        // =========================================================
        [HttpGet("GetServicePricingOptions/{serviceId:guid}")]
        public async Task<ActionResult<ApiResponse<List<ServicePricingOptionDto>>>> GetServicePricingOptions(Guid serviceId)
        {
            var userId = GetUserId();

            // Ensure service belongs to current influencer
            var owns = await _appDb.ServiceListings
                .AsNoTracking()
                .AnyAsync(x => x.Id == serviceId && x.InfluencerId == userId);

            if (!owns)
            {
                return NotFound(new ApiResponse<List<ServicePricingOptionDto>>
                {
                    Success = false,
                    Message = "Service listing not found."
                });
            }

            var entities = await _appDb.ServicePricingOptions
                .AsNoTracking()
                .Where(x => x.ServiceListingId == serviceId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<ServicePricingOptionDto>>(entities);

            return Ok(new ApiResponse<List<ServicePricingOptionDto>>
            {
                Success = true,
                Message = "Service pricing options loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // DELETE: api/ServicePricingOptions/DeleteServicePricingOption/{id}
        // =========================================================
        [HttpDelete("DeleteServicePricingOption/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteServicePricingOption(Guid id)
        {
            var userId = GetUserId();

            var option = await _appDb.ServicePricingOptions
                .IgnoreQueryFilters()
                .Include(x => x.ServiceListing)
                .FirstOrDefaultAsync(x => x.Id == id && x.ServiceListing.InfluencerId == userId);

            if (option == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Service pricing option not found."
                });
            }

            var ok = await _pricingOptions.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete service pricing option."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service pricing option deleted successfully."
            });
        }
    }
}