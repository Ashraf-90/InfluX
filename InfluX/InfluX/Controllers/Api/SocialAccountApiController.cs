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
    [Route("api/SocialAccounts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SocialAccountApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly ISocialAccountServices _socialAccounts;
        private readonly IMapper _mapper;

        public SocialAccountApiController(
            AppDBContext appDb,
            ISocialAccountServices socialAccounts,
            IMapper mapper)
        {
            _appDb = appDb;
            _socialAccounts = socialAccounts;
            _mapper = mapper;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/social-accounts/GetMySocialAccounts
        // =========================================================
        [HttpGet("GetSocialAccounts")]
        public async Task<ActionResult<ApiResponse<List<SocialAccountDto>>>> GetMySocialAccounts()
        {
            var userId = GetUserId();

            var entities = await _appDb.SocialAccounts
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            var dto = _mapper.Map<List<SocialAccountDto>>(entities);

            return Ok(new ApiResponse<List<SocialAccountDto>>
            {
                Success = true,
                Message = "Social accounts loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/social-accounts/CreateSocialAccount
        // =========================================================
        [HttpPost("CreateSocialAccount")]
        public async Task<ActionResult<ApiResponse<object>>> CreateSocialAccount(
            [FromBody] SocialAccountCreateDto req)
        {
            var userId = GetUserId();
            req.UserId = userId;

            var ok = await _socialAccounts.Create(req);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create social account."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Social account created successfully."
            });
        }

        // =========================================================
        // PUT: api/social-accounts/UpdateSocialAccount/{id}
        // =========================================================
        [HttpPut("UpdateSocialAccount/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateSocialAccount(
            Guid id,
            [FromBody] SocialAccountUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.SocialAccounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Social account not found."
                });
            }

            req.Id = id;

            var ok = await _socialAccounts.Update(req);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update social account."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Social account updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/social-accounts/DeleteSocialAccount/{id}
        // =========================================================
        [HttpDelete("DeleteSocialAccount/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSocialAccount(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.SocialAccounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Social account not found."
                });
            }

            var ok = await _socialAccounts.SoftDelete(id);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete social account."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Social account deleted successfully."
            });
        }
    }
}
