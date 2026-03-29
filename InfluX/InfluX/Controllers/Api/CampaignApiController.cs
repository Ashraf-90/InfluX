using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/Campaigns")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CampaignApiController : ControllerBase
    {
        private readonly ICampaignServices _campaigns;

        public CampaignApiController(ICampaignServices campaigns)
        {
            _campaigns = campaigns;
        }

        [HttpGet("GetAllCampaigns")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CampaignDto>>>> GetAllCampaigns()
        {
            var data = await _campaigns.GetAll();

            return Ok(new ApiResponse<IEnumerable<CampaignDto>>
            {
                Success = true,
                Message = "Campaigns loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetCampaignById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<CampaignDto>>> GetCampaignById(Guid id)
        {
            var data = await _campaigns.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<CampaignDto>
                {
                    Success = false,
                    Message = "Campaign not found."
                });
            }

            return Ok(new ApiResponse<CampaignDto>
            {
                Success = true,
                Message = "Campaign loaded successfully.",
                Data = data
            });
        }

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

            var ok = await _campaigns.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create campaign."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign created successfully."
            });
        }

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

            req.Id = id;

            var ok = await _campaigns.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign updated successfully."
            });
        }

        [HttpDelete("DeleteCampaign/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCampaign(Guid id)
        {
            var ok = await _campaigns.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign deleted successfully."
            });
        }
    }
}