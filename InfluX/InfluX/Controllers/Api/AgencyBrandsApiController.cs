using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/AgencyBrands")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
    public class AgencyBrandsApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgencyBrandServices _agencyBrandServices;

        public AgencyBrandsApiController(
            AppDBContext appDb,
            UserManager<ApplicationUser> userManager,
            IAgencyBrandServices agencyBrandServices)
        {
            _appDb = appDb;
            _userManager = userManager;
            _agencyBrandServices = agencyBrandServices;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        private async Task<AgencyProfile?> GetMyAgencyProfileAsync()
        {
            var userId = GetUserId();

            return await _appDb.AgencyProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        // GET: api/AgencyBrands/GetMyAgencyBrands
        [HttpGet("GetMyAgencyBrands")]
        public async Task<ActionResult<ApiResponse<object>>> GetMyAgencyBrands()
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

            var data = await _appDb.AgencyBrands
                .AsNoTracking()
                .Include(x => x.Brand)
                .Where(x => x.AgencyId == agency.Id)
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new
                {
                    id = x.Id,
                    agencyId = x.AgencyId,
                    brandId = x.BrandId,
                    brandName = x.Brand.BrandName,
                    website = x.Brand.Website,
                    description = x.Brand.Description,
                    logoUrl = x.Brand.LogoUrl,
                    industry = x.Brand.Industry,
                    country = x.Brand.Country,
                    city = x.Brand.City,
                    active = x.Active,
                    isAvilable = x.IsAvilable,
                    createDate = x.CreateDate,
                    updateDate = x.UpdateDate
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Agency brands loaded successfully.",
                Data = data
            });
        }

        // GET: api/AgencyBrands/GetAvailableBrands
        [HttpGet("GetAvailableBrands")]
        public async Task<ActionResult<ApiResponse<object>>> GetAvailableBrands()
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

            var linkedBrandIds = await _appDb.AgencyBrands
                .AsNoTracking()
                .Where(x => x.AgencyId == agency.Id)
                .Select(x => x.BrandId)
                .ToListAsync();

            var brands = await _appDb.BrandProfiles
                .AsNoTracking()
                .Where(x => !linkedBrandIds.Contains(x.Id))
                .OrderBy(x => x.BrandName)
                .Select(x => new
                {
                    id = x.Id,
                    userId = x.UserId,
                    brandName = x.BrandName,
                    website = x.Website,
                    description = x.Description,
                    logoUrl = x.LogoUrl,
                    industry = x.Industry,
                    country = x.Country,
                    city = x.City
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Available brands loaded successfully.",
                Data = brands
            });
        }

        // POST: api/AgencyBrands/AttachExistingBrand
        [HttpPost("AttachExistingBrand")]
        public async Task<ActionResult<ApiResponse<object>>> AttachExistingBrand([FromBody] AttachExistingBrandToAgencyDto req)
        {
            if (req == null || req.BrandId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "brandId is required."
                });
            }

            var agency = await GetMyAgencyProfileAsync();
            if (agency == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Agency profile not found."
                });
            }

            var brand = await _appDb.BrandProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == req.BrandId);

            if (brand == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid brandId."
                });
            }

            var exists = await _appDb.AgencyBrands
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.AgencyId == agency.Id && x.BrandId == req.BrandId);

            if (exists != null && exists.Active)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "This brand is already linked to your agency."
                });
            }

            if (exists != null && !exists.Active)
            {
                exists.Active = true;
                exists.UpdateDate = DateTime.UtcNow;
                await _appDb.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Brand re-linked to agency successfully."
                });
            }

            var ok = await _agencyBrandServices.Create(new AgencyBrandCreateDto
            {
                AgencyId = agency.Id,
                BrandId = req.BrandId
            });

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to link brand to agency."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Brand linked to agency successfully."
            });
        }

        // POST: api/AgencyBrands/CreateBrandAndAttach
        [HttpPost("CreateBrandAndAttach")]
        public async Task<ActionResult<ApiResponse<object>>> CreateBrandAndAttach([FromBody] CreateBrandAndAttachDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var agency = await GetMyAgencyProfileAsync();
            if (agency == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Agency profile not found."
                });
            }

            var emailExists = await _userManager.FindByEmailAsync(req.Email);
            if (emailExists != null)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Email already registered."
                });
            }

            ApplicationUser? createdUser = null;

            try
            {
                createdUser = new ApplicationUser
                {
                    UserName = req.Email,
                    Email = req.Email,
                    PhoneNumber = req.PhoneNumber,
                    AppRole = "Brand",
                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                var createUserRes = await _userManager.CreateAsync(createdUser, req.Password);
                if (!createUserRes.Succeeded)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = string.Join(" | ", createUserRes.Errors.Select(e => e.Description))
                    });
                }

                try
                {
                    await _userManager.AddToRoleAsync(createdUser, "Brand");
                }
                catch
                {
                    // تجاهل في حال لم يكن الدور موجوداً داخل Identity DB
                }

                var userProfile = new UserProfile
                {
                    UserId = createdUser.Id,
                    FullName = req.FullName,
                    Country = req.Country,
                    City = req.City,
                    Language = req.Language,
                    AvatarUrl = req.AvatarUrl,
                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _appDb.UserProfiles.Add(userProfile);
                await _appDb.SaveChangesAsync();

                var brandProfile = new BrandProfile
                {
                    UserId = createdUser.Id,
                    BrandName = req.BrandName,
                    Website = req.Website,
                    Description = req.Description,
                    LogoUrl = req.LogoUrl,
                    Industry = req.Industry,
                    Country = req.Country,
                    City = req.City,
                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _appDb.BrandProfiles.Add(brandProfile);
                await _appDb.SaveChangesAsync();

                var agencyBrand = new AgencyBrand
                {
                    AgencyId = agency.Id,
                    BrandId = brandProfile.Id,
                    Active = true,
                    IsAvilable = true,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                _appDb.AgencyBrands.Add(agencyBrand);
                await _appDb.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Brand created and attached to agency successfully.",
                    Data = new
                    {
                        agencyBrandId = agencyBrand.Id,
                        brandId = brandProfile.Id,
                        brandUserId = createdUser.Id,
                        email = createdUser.Email
                    }
                });
            }
            catch (Exception ex)
            {
                if (createdUser != null)
                {
                    try
                    {
                        var existingUser = await _userManager.FindByEmailAsync(createdUser.Email!);
                        if (existingUser != null)
                        {
                            await _userManager.DeleteAsync(existingUser);
                        }
                    }
                    catch
                    {
                    }
                }

                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Failed to create brand and attach it. {ex.Message}"
                });
            }
        }

        // DELETE: api/AgencyBrands/RemoveBrandFromAgency/{id}
        [HttpDelete("RemoveBrandFromAgency/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> RemoveBrandFromAgency(Guid id)
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

            var relation = await _appDb.AgencyBrands
                .FirstOrDefaultAsync(x => x.Id == id && x.AgencyId == agency.Id);

            if (relation == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Agency brand link not found."
                });
            }

            relation.Active = false;
            relation.UpdateDate = DateTime.UtcNow;

            await _appDb.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Brand removed from agency successfully."
            });
        }
    }
}