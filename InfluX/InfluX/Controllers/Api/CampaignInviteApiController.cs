using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/CampaignInvites")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CampaignInviteApiController : ControllerBase
    {
        private readonly ICampaignInviteServices _invites;

        public CampaignInviteApiController(ICampaignInviteServices invites)
        {
            _invites = invites;
        }

        [HttpGet("GetAllCampaignInvites")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CampaignInviteDto>>>> GetAllCampaignInvites()
        {
            var data = await _invites.GetAll();

            return Ok(new ApiResponse<IEnumerable<CampaignInviteDto>>
            {
                Success = true,
                Message = "Campaign invites loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetCampaignInviteById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<CampaignInviteDto>>> GetCampaignInviteById(Guid id)
        {
            var data = await _invites.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<CampaignInviteDto>
                {
                    Success = false,
                    Message = "Campaign invite not found."
                });
            }

            return Ok(new ApiResponse<CampaignInviteDto>
            {
                Success = true,
                Message = "Campaign invite loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateCampaignInvite")]
        public async Task<ActionResult<ApiResponse<object>>> CreateCampaignInvite([FromBody] CampaignInviteCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _invites.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create campaign invite."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign invite created successfully."
            });
        }

        [HttpPut("UpdateCampaignInvite/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateCampaignInvite(Guid id, [FromBody] CampaignInviteUpdateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            req.Id = id;

            var ok = await _invites.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign invite not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign invite updated successfully."
            });
        }

        [HttpDelete("DeleteCampaignInvite/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCampaignInvite(Guid id)
        {
            var ok = await _invites.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign invite not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign invite deleted successfully."
            });
        }
    }
}