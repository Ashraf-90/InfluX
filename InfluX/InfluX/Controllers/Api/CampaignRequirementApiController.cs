using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/CampaignRequirements")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CampaignRequirementApiController : ControllerBase
    {
        private readonly ICampaignRequirementServices _requirements;

        public CampaignRequirementApiController(ICampaignRequirementServices requirements)
        {
            _requirements = requirements;
        }

        [HttpGet("GetAllCampaignRequirements")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CampaignRequirementDto>>>> GetAllCampaignRequirements()
        {
            var data = await _requirements.GetAll();

            return Ok(new ApiResponse<IEnumerable<CampaignRequirementDto>>
            {
                Success = true,
                Message = "Campaign requirements loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetCampaignRequirementById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<CampaignRequirementDto>>> GetCampaignRequirementById(Guid id)
        {
            var data = await _requirements.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<CampaignRequirementDto>
                {
                    Success = false,
                    Message = "Campaign requirement not found."
                });
            }

            return Ok(new ApiResponse<CampaignRequirementDto>
            {
                Success = true,
                Message = "Campaign requirement loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateCampaignRequirement")]
        public async Task<ActionResult<ApiResponse<object>>> CreateCampaignRequirement([FromBody] CampaignRequirementCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _requirements.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create campaign requirement."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign requirement created successfully."
            });
        }

        [HttpPut("UpdateCampaignRequirement/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateCampaignRequirement(Guid id, [FromBody] CampaignRequirementUpdateDto req)
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

            var ok = await _requirements.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign requirement not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign requirement updated successfully."
            });
        }

        [HttpDelete("DeleteCampaignRequirement/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteCampaignRequirement(Guid id)
        {
            var ok = await _requirements.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Campaign requirement not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Campaign requirement deleted successfully."
            });
        }
    }
}