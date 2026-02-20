using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/VerificationRequests")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VerificationRequestApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IVerificationRequestServices _verification;

        public VerificationRequestApiController(AppDBContext appDb, IVerificationRequestServices verification)
        {
            _appDb = appDb;
            _verification = verification;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // POST: api/VerificationRequests/CreateVerificationRequest
        // Body: VerificationRequestCreateDto  (UserId يتم أخذه من التوكن)
        // =========================================================
        [HttpPost("CreateVerificationRequest")]
        public async Task<ActionResult<ApiResponse<object>>> CreateVerificationRequest([FromBody] VerificationRequestCreateDto req)
        {
            if (req == null)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid request body." });
            }

            req.UserId = GetUserId(); // فرض UserId من التوكن :contentReference[oaicite:6]{index=6}

            var ok = await _verification.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to create verification request." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Verification request created successfully." });
        }

        // =========================================================
        // GET: api/VerificationRequests/GetMyVerificationRequests
        // =========================================================
        [HttpGet("GetVerificationRequests")]
        public async Task<ActionResult<ApiResponse<List<VerificationRequestDto>>>> GetMyVerificationRequests()
        {
            var userId = GetUserId();

            var list = await _appDb.VerificationRequests
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            // AutoMapper موجود، لكن هنا نرجّع DTO بسيط بدون Include
            var dto = list.Select(x => new VerificationRequestDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Status = x.Status,
                DocType = x.DocType,
                DocUrl = x.DocUrl,
                Notes = x.Notes,
                Active = x.Active,
                IsAvilable = x.IsAvilable,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate
            }).ToList();

            return Ok(new ApiResponse<List<VerificationRequestDto>>
            {
                Success = true,
                Message = "Verification requests loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // GET: api/VerificationRequests/GetVerificationRequestById/{id}
        // (لصاحب الطلب فقط)
        // =========================================================
        [HttpGet("GetVerificationRequestById/{id:guid}")]
        public async Task<ActionResult<ApiResponse<VerificationRequestDto>>> GetVerificationRequestById(Guid id)
        {
            var userId = GetUserId();

            var x = await _appDb.VerificationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id && v.UserId == userId);

            if (x == null)
            {
                return NotFound(new ApiResponse<VerificationRequestDto> { Success = false, Message = "Verification request not found." });
            }

            var dto = new VerificationRequestDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Status = x.Status,
                DocType = x.DocType,
                DocUrl = x.DocUrl,
                Notes = x.Notes,
                Active = x.Active,
                IsAvilable = x.IsAvilable,
                CreateDate = x.CreateDate,
                UpdateDate = x.UpdateDate
            };

            return Ok(new ApiResponse<VerificationRequestDto>
            {
                Success = true,
                Message = "Verification request loaded successfully.",
                Data = dto
            });
        }

        // =========================================================
        // PUT: api/VerificationRequests/UpdateMyVerificationRequest/{id}
        // المستخدم يعدّل Notes فقط (Status يتم تثبيته Pending)
        // Body: VerificationRequestUpdateDto
        // =========================================================
        [HttpPut("UpdateVerificationRequest/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateMyVerificationRequest(Guid id, [FromBody] VerificationRequestUpdateDto req)
        {
            var userId = GetUserId();

            var entity = await _appDb.VerificationRequests
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Verification request not found." });
            }

            // لا نسمح للمستخدم يغير Status
            req.Id = id;
            req.Status = VerificationStatus.Pending; // Pending=1 :contentReference[oaicite:7]{index=7}

            var ok = await _verification.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to update verification request." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Verification request updated successfully." });
        }

        // =========================================================
        // PUT: api/VerificationRequests/ReviewVerificationRequest/{id}
        // للمراجعة (تغيير Status Approved/Rejected)
        // NOTE: غيّر Roles حسب نظامك
        // =========================================================
        [HttpPut("ReviewVerificationRequest/{id:guid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Agency")]
        public async Task<ActionResult<ApiResponse<object>>> ReviewVerificationRequest(Guid id, [FromBody] VerificationRequestUpdateDto req)
        {
            var entity = await _appDb.VerificationRequests
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Verification request not found." });
            }

            req.Id = id; // Status يأتي من الـ body (Approved/Rejected)

            var ok = await _verification.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to review verification request." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Verification request reviewed successfully." });
        }

        // =========================================================
        // DELETE: api/VerificationRequests/DeleteVerificationRequest/{id}
        // (لصاحب الطلب فقط) Soft Delete
        // =========================================================
        [HttpDelete("DeleteVerificationRequest/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVerificationRequest(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.VerificationRequests
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object> { Success = false, Message = "Verification request not found." });
            }

            var ok = await _verification.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Message = "Failed to delete verification request." });
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "Verification request deleted successfully." });
        }
    }
}