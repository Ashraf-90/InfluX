using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/OrderApprovals")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderApprovalApiController : ControllerBase
    {
        private readonly IOrderApprovalServices _approvals;

        public OrderApprovalApiController(IOrderApprovalServices approvals)
        {
            _approvals = approvals;
        }

        [HttpGet("GetAllOrderApprovals")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderApprovalDto>>>> GetAllOrderApprovals()
        {
            var data = await _approvals.GetAll();

            return Ok(new ApiResponse<IEnumerable<OrderApprovalDto>>
            {
                Success = true,
                Message = "Order approvals loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetOrderApprovalById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderApprovalDto>>> GetOrderApprovalById(Guid id)
        {
            var data = await _approvals.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<OrderApprovalDto>
                {
                    Success = false,
                    Message = "Order approval not found."
                });
            }

            return Ok(new ApiResponse<OrderApprovalDto>
            {
                Success = true,
                Message = "Order approval loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateOrderApproval")]
        public async Task<ActionResult<ApiResponse<object>>> CreateOrderApproval([FromBody] OrderApprovalCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _approvals.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create order approval."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order approval created successfully."
            });
        }

        [HttpPut("UpdateOrderApproval/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateOrderApproval(Guid id, [FromBody] OrderApprovalUpdateDto req)
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

            var ok = await _approvals.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order approval not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order approval updated successfully."
            });
        }

        [HttpDelete("DeleteOrderApproval/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteOrderApproval(Guid id)
        {
            var ok = await _approvals.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order approval not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order approval deleted successfully."
            });
        }
    }
}