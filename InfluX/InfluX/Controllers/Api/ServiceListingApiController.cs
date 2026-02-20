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
    [Route("api/ServiceListings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiceListingApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IServiceListingServices _serviceListings;
        private readonly IMapper _mapper;

        public ServiceListingApiController(AppDBContext appDb,IServiceListingServices serviceListings,IMapper mapper)
        {
            _appDb = appDb;
            _serviceListings = serviceListings;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // POST: api/ServiceListings/CreateServiceListing
        // Body: ServiceListingCreateWithPricingOptionsDto
        // Creates ServiceListing + (optional) PricingOptions in ONE request
        // =========================================================
        [HttpPost("CreateServiceListing")]
        public async Task<ActionResult<ApiResponse<object>>> CreateServiceListing(
            [FromBody] ServiceListingCreateWithPricingOptionsDto req)
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

            // Create entity (InfluencerId from token)
            var listing = new ServiceListing
            {
                InfluencerId = userId,
                Title = req.Title,
                Description = req.Description,
                Platform = req.Platform,
                DeliverableType = req.DeliverableType,
                BasePrice = req.BasePrice,
                DurationDays = req.DurationDays,
                RevisionsCount = req.RevisionsCount,
                // Status default is Active in entity :contentReference[oaicite:5]{index=5}
            };

            // Optional pricing options
            if (req.PricingOptions != null && req.PricingOptions.Count > 0)
            {
                foreach (var op in req.PricingOptions)
                {
                    if (string.IsNullOrWhiteSpace(op.Key)) continue;

                    listing.PricingOptions.Add(new ServicePricingOption
                    {
                        Key = op.Key,
                        Price = op.Price,
                        Notes = op.Notes
                    });
                }
            }

            _appDb.ServiceListings.Add(listing);
            await _appDb.SaveChangesAsync(); // ApplyCommonDates will set CreateDate/UpdateDate :contentReference[oaicite:6]{index=6}

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service listing created successfully.",
                Data = new
                {
                    serviceListingId = listing.Id,
                    pricingOptionsCount = listing.PricingOptions.Count,
                    pricingOptionsIds = listing.PricingOptions.Select(x => x.Id).ToList()
                }
            });
        }

        // =========================================================
        // PUT: api/ServiceListings/UpdateServiceListing/{id}
        // Body: ServiceListingUpdateDto
        // =========================================================
        [HttpPut("UpdateServiceListing/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateServiceListing(Guid id,[FromBody] ServiceListingUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.ServiceListings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Service listing not found."
                });
            }

            req.Id = id;

            var ok = await _serviceListings.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update service listing."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service listing updated successfully."
            });
        }

        // =========================================================
        // GET: api/ServiceListings/GetServiceListing/{id}
        // Returns ServiceListing + PricingOptions
        // =========================================================
        [HttpGet("GetServiceListing/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> GetServiceListing(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.ServiceListings
                .AsNoTracking()
                .Include(x => x.PricingOptions)
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Service listing not found."
                });
            }

            var listingDto = _mapper.Map<ServiceListingDto>(entity);
            var optionsDto = _mapper.Map<List<ServicePricingOptionDto>>(entity.PricingOptions);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service listing loaded successfully.",
                Data = new
                {
                    service = listingDto,
                    pricingOptions = optionsDto
                }
            });
        }

        // =========================================================
        // GET: api/ServiceListings/GetServiceListings
        // Returns all service listings for current influencer (token)
        // =========================================================
        [HttpGet("GetServiceListings")]
        public async Task<ActionResult<ApiResponse<List<ServiceListingDto>>>> GetServiceListings()
        {
            var userId = GetUserId();

            var entities = await _appDb.ServiceListings
                .AsNoTracking()
                .Include(x => x.PricingOptions)
                .Where(x => x.InfluencerId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<ServiceListingDto>>(entities);

            return Ok(new ApiResponse<List<ServiceListingDto>>
            {
                Success = true,
                Message = "Service listings loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // DELETE: api/ServiceListings/DeleteServiceListing/{id}
        // Soft-delete listing + soft-delete its pricing options
        // =========================================================
        [HttpDelete("DeleteServiceListing/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteServiceListing(Guid id)
        {
            var userId = GetUserId();

            var listing = await _appDb.ServiceListings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.InfluencerId == userId);

            if (listing == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Service listing not found."
                });
            }

            // Soft delete listing
            var ok = await _serviceListings.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete service listing."
                });
            }

            // Soft delete related pricing options (important)
            var options = await _appDb.ServicePricingOptions
                .IgnoreQueryFilters()
                .Where(x => x.ServiceListingId == id)
                .ToListAsync();

            foreach (var op in options)
                op.Active = false;

            await _appDb.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Service listing deleted successfully."
            });
        }
    }
}