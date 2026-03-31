using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/OrderDeliverables")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderDeliverableApiController : ControllerBase
    {
        private readonly IOrderDeliverableServices _deliverables;

        public OrderDeliverableApiController(IOrderDeliverableServices deliverables)
        {
            _deliverables = deliverables;
        }

        [HttpGet("GetAllOrderDeliverables")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDeliverableDto>>>> GetAllOrderDeliverables()
        {
            var data = await _deliverables.GetAll();

            return Ok(new ApiResponse<IEnumerable<OrderDeliverableDto>>
            {
                Success = true,
                Message = "Order deliverables loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetOrderDeliverableById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDeliverableDto>>> GetOrderDeliverableById(Guid id)
        {
            var data = await _deliverables.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<OrderDeliverableDto>
                {
                    Success = false,
                    Message = "Order deliverable not found."
                });
            }

            return Ok(new ApiResponse<OrderDeliverableDto>
            {
                Success = true,
                Message = "Order deliverable loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateOrderDeliverable")]
        public async Task<ActionResult<ApiResponse<object>>> CreateOrderDeliverable([FromBody] OrderDeliverableCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _deliverables.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create order deliverable."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order deliverable created successfully."
            });
        }

        [HttpPut("UpdateOrderDeliverable/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateOrderDeliverable(Guid id, [FromBody] OrderDeliverableUpdateDto req)
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

            var ok = await _deliverables.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order deliverable not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order deliverable updated successfully."
            });
        }

        [HttpDelete("DeleteOrderDeliverable/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteOrderDeliverable(Guid id)
        {
            var ok = await _deliverables.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order deliverable not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order deliverable deleted successfully."
            });
        }
    }
}