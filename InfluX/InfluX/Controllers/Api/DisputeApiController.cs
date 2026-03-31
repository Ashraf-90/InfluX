using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/Disputes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DisputeApiController : ControllerBase
    {
        private readonly IDisputeServices _disputes;

        public DisputeApiController(IDisputeServices disputes)
        {
            _disputes = disputes;
        }

        [HttpGet("GetAllDisputes")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DisputeDto>>>> GetAllDisputes()
        {
            var data = await _disputes.GetAll();

            return Ok(new ApiResponse<IEnumerable<DisputeDto>>
            {
                Success = true,
                Message = "Disputes loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetDisputeById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<DisputeDto>>> GetDisputeById(Guid id)
        {
            var data = await _disputes.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<DisputeDto>
                {
                    Success = false,
                    Message = "Dispute not found."
                });
            }

            return Ok(new ApiResponse<DisputeDto>
            {
                Success = true,
                Message = "Dispute loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateDispute")]
        public async Task<ActionResult<ApiResponse<object>>> CreateDispute([FromBody] DisputeCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _disputes.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create dispute."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dispute created successfully."
            });
        }

        [HttpPut("UpdateDispute/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateDispute(Guid id, [FromBody] DisputeUpdateDto req)
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

            var ok = await _disputes.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Dispute not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dispute updated successfully."
            });
        }

        [HttpDelete("DeleteDispute/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteDispute(Guid id)
        {
            var ok = await _disputes.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Dispute not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Dispute deleted successfully."
            });
        }
    }
}