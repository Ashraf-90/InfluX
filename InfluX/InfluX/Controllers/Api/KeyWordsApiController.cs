using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/KeyWords")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class KeyWordsApiController : ControllerBase
    {
        private readonly IKeyWordsServices _keyWordsServices;

        public KeyWordsApiController(IKeyWordsServices keyWordsServices)
        {
            _keyWordsServices = keyWordsServices;
        }

        // =========================================================
        // GET: api/KeyWords/GetAllKeyWords
        // =========================================================
        [HttpGet("GetAllKeyWords")]
        public async Task<ActionResult<ApiResponse<List<KeyWordsDto>>>> GetAllKeyWords()
        {
            var list = (await _keyWordsServices.GetAllKeyWords()).ToList(); // interface returns IEnumerable :contentReference[oaicite:4]{index=4}

            return Ok(new ApiResponse<List<KeyWordsDto>>
            {
                Success = true,
                Message = "KeyWords loaded successfully.",
                Data = list
            });
        }

        // =========================================================
        // GET: api/KeyWords/GetKeyWordById/{id}
        // =========================================================
        [HttpGet("GetKeyWordById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<KeyWordsDto>>> GetKeyWordById(Guid id)
        {
            var dto = await _keyWordsServices.GetById(id);
            if (dto == null)
            {
                return NotFound(new ApiResponse<KeyWordsDto>
                {
                    Success = false,
                    Message = "KeyWord not found."
                });
            }

            return Ok(new ApiResponse<KeyWordsDto>
            {
                Success = true,
                Message = "KeyWord loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // POST: api/KeyWords/CreateKeyWord
        // Body: { "enKeyword": "...", "arKeyword": "..." }
        // =========================================================
        [HttpPost("CreateKeyWord")]
        public async Task<ActionResult<ApiResponse<object>>> CreateKeyWord([FromBody] KeyWordsCreateDto req) // :contentReference[oaicite:5]{index=5}
        {
            var ok = await _keyWordsServices.AddNewKeyWords(req); // :contentReference[oaicite:6]{index=6}
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Keyword created successfully."
            });
        }

        // =========================================================
        // PUT: api/KeyWords/UpdateKeyWord/{id}
        // =========================================================
        [HttpPut("UpdateKeyWord/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateKeyWord(Guid id, [FromBody] KeyWordsUpdateDto req) // :contentReference[oaicite:7]{index=7}
        {
            req.Id = id;

            var ok = await _keyWordsServices.UpdateKeyWords(req); // :contentReference[oaicite:8]{index=8}
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Keyword updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/KeyWords/DeleteKeyWord/{id}
        // =========================================================
        [HttpDelete("DeleteKeyWord/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteKeyWord(Guid id)
        {
            var ok = await _keyWordsServices.DeleteKeyWordsAsync(id); // :contentReference[oaicite:9]{index=9}
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Keyword deleted successfully."
            });
        }
    }
}
