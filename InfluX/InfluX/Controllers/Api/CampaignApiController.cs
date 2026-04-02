using Application.DTOs;
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
    [Route("api/Campaigns")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency,Brand")]
    public class CampaignApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;

        public CampaignApiController(AppDBContext appDb)
        {
            _appDb = appDb;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        private bool IsAgency() => User.IsInRole("Agency");
        private bool IsBrand() => User.IsInRole("Brand");

        private async Task<AgencyProfile?> GetMyAgencyProfileAsync()
        {
            var userId = GetUserId();

            return await _appDb.AgencyProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        private async Task<BrandProfile?> GetMyBrandProfileAsync()
        {
            var userId = GetUserId();

            return await _appDb.BrandProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        private static object ToViewModel(Campaign x)
        {
            return new
            {
                id = x.Id,
                brandProfileId = x.BrandProfileId,
                agencyProfileId = x.AgencyProfileId,
                brandName = x.BrandProfile.BrandName,
                agencyName = x.AgencyProfile != null ? x.AgencyProfile.AgencyName : null,
                title = x.Title,
                objective = x.Objective,
                totalBudget = x.TotalBudget,
                startDate = x.StartDate,
                endDate = x.EndDate,
                status = x.Status,
                createdBy = x.AgencyProfileId.HasValue ? "Agency" : "Brand",
                active = x.Active,
                isAvilable = x.IsAvilable,
                createDate = x.CreateDate,
                updateDate = x.UpdateDate
            };
        }

        // GET: api/Campaigns/GetAllCampaigns
        [HttpGet("GetAllCampaigns")]
        public async Task<ActionResult<ApiResponse<object>>> GetAllCampaigns()
        {
            IQueryable<Campaign> query = _appDb.Campaigns
                .AsNoTracking()
                .Include(x => x.BrandProfile)
                .Include(x => x.AgencyProfile)
                .OrderByDescending(x => x.CreateDate);

            if (IsAgency())
            {
                var agency = await GetMyAgencyProfileAsync();
                if (agency == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency profile not found."
                    });
                }

                query = query.Where(x => x.AgencyProfileId == agency.Id);
            }
            else if (IsBrand())
            {
                var brand = await GetMyBrandProfileAsync();
                if (brand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Brand profile not found."
                    });
                }

                query = query.Where(x => x.BrandProfileId == brand.Id && x.AgencyProfileId == null);
            }
            else
            {
                return Forbid();
            }

            var data = await query.ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaigns loaded successfully.",
                Data = data.Select(ToViewModel).ToList()
            });
        }

        // GET: api/Campaigns/GetCampaignById/{id}
        [HttpGet("GetCampaignById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> GetCampaignById(Guid id)
        {
            var campaign = await _appDb.Campaigns
                .AsNoTracking()
                .Include(x => x.BrandProfile)
                .Include(x => x.AgencyProfile)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign not found."
                });
            }

            if (IsAgency())
            {
                var agency = await GetMyAgencyProfileAsync();
                if (agency == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency profile not found."
                    });
                }

                if (campaign.AgencyProfileId != agency.Id)
                {
                    return Forbid();
                }
            }
            else if (IsBrand())
            {
                var brand = await GetMyBrandProfileAsync();
                if (brand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Brand profile not found."
                    });
                }

                if (campaign.BrandProfileId != brand.Id || campaign.AgencyProfileId != null)
                {
                    return Forbid();
                }
            }
            else
            {
                return Forbid();
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign loaded successfully.",
                Data = ToViewModel(campaign)
            });
        }

        // POST: api/Campaigns/CreateCampaign
        [HttpPost("CreateCampaign")]
        public async Task<ActionResult<ApiResponse<object>>> CreateCampaign([FromBody] CampaignCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            if (req.BrandProfileId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "BrandProfileId is required."
                });
            }

            if (string.IsNullOrWhiteSpace(req.Title))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Title is required."
                });
            }

            if (req.TotalBudget < 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "TotalBudget cannot be negative."
                });
            }

            if (req.EndDate < req.StartDate)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "EndDate cannot be earlier than StartDate."
                });
            }

            var brand = await _appDb.BrandProfiles
                .FirstOrDefaultAsync(x => x.Id == req.BrandProfileId);

            if (brand == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid BrandProfileId."
                });
            }

            Guid? agencyProfileId = null;

            if (IsAgency())
            {
                var agency = await GetMyAgencyProfileAsync();
                if (agency == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency profile not found."
                    });
                }

                if (!req.AgencyProfileId.HasValue || req.AgencyProfileId.Value == Guid.Empty)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "AgencyProfileId is required when the campaign is created by Agency."
                    });
                }

                if (req.AgencyProfileId.Value != agency.Id)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "You can only create campaigns using your own AgencyProfileId."
                    });
                }

                agencyProfileId = agency.Id;
            }
            else if (IsBrand())
            {
                var myBrand = await GetMyBrandProfileAsync();
                if (myBrand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Brand profile not found."
                    });
                }

                if (req.BrandProfileId != myBrand.Id)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "You can only create campaigns for your own BrandProfileId."
                    });
                }

                if (req.AgencyProfileId.HasValue && req.AgencyProfileId.Value != Guid.Empty)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "AgencyProfileId must be null when the campaign is created by Brand."
                    });
                }

                agencyProfileId = null;
            }
            else
            {
                return Forbid();
            }

            var campaign = new Campaign
            {
                BrandProfileId = req.BrandProfileId,
                AgencyProfileId = agencyProfileId,
                Title = req.Title.Trim(),
                Objective = string.IsNullOrWhiteSpace(req.Objective) ? null : req.Objective.Trim(),
                TotalBudget = req.TotalBudget,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Status = req.Status,
                Active = req.Active,
                IsAvilable = req.IsAvilable
            };

            _appDb.Campaigns.Add(campaign);
            await _appDb.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign created successfully.",
                Data = new
                {
                    campaignId = campaign.Id,
                    brandProfileId = campaign.BrandProfileId,
                    agencyProfileId = campaign.AgencyProfileId,
                    createdBy = campaign.AgencyProfileId.HasValue ? "Agency" : "Brand"
                }
            });
        }

        // PUT: api/Campaigns/UpdateCampaign/{id}
        [HttpPut("UpdateCampaign/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateCampaign(Guid id, [FromBody] CampaignUpdateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var campaign = await _appDb.Campaigns
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign not found."
                });
            }

            if (req.BrandProfileId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "BrandProfileId is required."
                });
            }

            if (string.IsNullOrWhiteSpace(req.Title))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Title is required."
                });
            }

            if (req.TotalBudget < 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "TotalBudget cannot be negative."
                });
            }

            if (req.EndDate < req.StartDate)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "EndDate cannot be earlier than StartDate."
                });
            }

            var brand = await _appDb.BrandProfiles
                .FirstOrDefaultAsync(x => x.Id == req.BrandProfileId);

            if (brand == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid BrandProfileId."
                });
            }

            if (IsAgency())
            {
                var agency = await GetMyAgencyProfileAsync();
                if (agency == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency profile not found."
                    });
                }

                if (campaign.AgencyProfileId != agency.Id)
                {
                    return Forbid();
                }

                if (!req.AgencyProfileId.HasValue || req.AgencyProfileId.Value == Guid.Empty)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "AgencyProfileId is required when the campaign is updated by Agency."
                    });
                }

                if (req.AgencyProfileId.Value != agency.Id)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "You can only update campaigns using your own AgencyProfileId."
                    });
                }

                campaign.AgencyProfileId = agency.Id;
                campaign.BrandProfileId = req.BrandProfileId;
            }
            else if (IsBrand())
            {
                var myBrand = await GetMyBrandProfileAsync();
                if (myBrand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Brand profile not found."
                    });
                }

                if (campaign.BrandProfileId != myBrand.Id || campaign.AgencyProfileId != null)
                {
                    return Forbid();
                }

                if (req.BrandProfileId != myBrand.Id)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "You can only update campaigns for your own BrandProfileId."
                    });
                }

                if (req.AgencyProfileId.HasValue && req.AgencyProfileId.Value != Guid.Empty)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "AgencyProfileId must remain null for brand-created campaigns."
                    });
                }

                campaign.AgencyProfileId = null;
                campaign.BrandProfileId = myBrand.Id;
            }
            else
            {
                return Forbid();
            }

            campaign.Title = req.Title.Trim();
            campaign.Objective = string.IsNullOrWhiteSpace(req.Objective) ? null : req.Objective.Trim();
            campaign.TotalBudget = req.TotalBudget;
            campaign.StartDate = req.StartDate;
            campaign.EndDate = req.EndDate;
            campaign.Status = req.Status;
            campaign.Active = req.Active;
            campaign.IsAvilable = req.IsAvilable;

            await _appDb.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign updated successfully."
            });
        }

        // DELETE: api/Campaigns/DeleteCampaign/{id}
        [HttpDelete("DeleteCampaign/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCampaign(Guid id)
        {
            var campaign = await _appDb.Campaigns
                .FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign not found."
                });
            }

            if (IsAgency())
            {
                var agency = await GetMyAgencyProfileAsync();
                if (agency == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency profile not found."
                    });
                }

                if (campaign.AgencyProfileId != agency.Id)
                {
                    return Forbid();
                }
            }
            else if (IsBrand())
            {
                var brand = await GetMyBrandProfileAsync();
                if (brand == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Brand profile not found."
                    });
                }

                if (campaign.BrandProfileId != brand.Id || campaign.AgencyProfileId != null)
                {
                    return Forbid();
                }
            }
            else
            {
                return Forbid();
            }

            campaign.Active = false;
            await _appDb.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign deleted successfully."
            });
        }
    }
}