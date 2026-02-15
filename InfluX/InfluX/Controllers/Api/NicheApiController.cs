using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/niches")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NicheApiController : ControllerBase
    {
        private readonly INicheServices _nicheServices;

        public NicheApiController(INicheServices nicheServices)
        {
            _nicheServices = nicheServices;
        }

        // =========================================================
        // POST: api/niches/CreateNiche
        // =========================================================
        [HttpPost("CreateNiche")]
        public async Task<ActionResult<ApiResponse<object>>> CreateNiche(
            [FromBody] NicheCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid request body."
                });
            }

            var ok = await _nicheServices.Create(req);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Niche created successfully."
            });
        }

        // =========================================================
        // PUT: api/niches/UpdateNiche/{id}
        // =========================================================
        [HttpPut("UpdateNiche/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateNiche(
            Guid id,
            [FromBody] NicheUpdateDto req)
        {
            req.Id = id;

            var ok = await _nicheServices.Update(req);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Niche updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/niches/DeleteNiche/{id}
        // =========================================================
        [HttpDelete("DeleteNiche/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteNiche(Guid id)
        {
            var ok = await _nicheServices.SoftDelete(id);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Niche deleted successfully."
            });
        }

        // =========================================================
        // GET: api/niches/GetAllNiches
        // =========================================================
        [HttpGet("GetAllNiches")]
        public async Task<ActionResult<ApiResponse<List<NicheDto>>>> GetAllNiches()
        {
            var list = (await _nicheServices.GetAll()).ToList();

            return Ok(new ApiResponse<List<NicheDto>>
            {
                Success = true,
                Message = "Niches loaded successfully.",
                Data = list
            });
        }
    }
}
