using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/Orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderServices _orders;

        public OrderApiController(IOrderServices orders)
        {
            _orders = orders;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAllOrders()
        {
            var data = await _orders.GetAll();

            return Ok(new ApiResponse<IEnumerable<OrderDto>>
            {
                Success = true,
                Message = "Orders loaded successfully.",
                Data = data
            });
        }

        [HttpGet("GetOrderById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(Guid id)
        {
            var data = await _orders.GetById(id);
            if (data == null)
            {
                return NotFound(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Order not found."
                });
            }

            return Ok(new ApiResponse<OrderDto>
            {
                Success = true,
                Message = "Order loaded successfully.",
                Data = data
            });
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<ApiResponse<object>>> CreateOrder([FromBody] OrderCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _orders.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create order."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order created successfully."
            });
        }

        [HttpPut("UpdateOrder/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateOrder(Guid id, [FromBody] OrderUpdateDto req)
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

            var ok = await _orders.Update(req);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order not found or update failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order updated successfully."
            });
        }

        [HttpDelete("DeleteOrder/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteOrder(Guid id)
        {
            var ok = await _orders.SoftDelete(id);
            if (!ok)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Order not found or delete failed."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Order deleted successfully."
            });
        }
    }
}