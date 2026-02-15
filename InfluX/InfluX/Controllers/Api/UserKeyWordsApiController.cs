using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfluX.Controllers.Api
{
    [ApiController]
    [Route("api/UserKeyWords")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserKeyWordsApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IUserKeyWordServices _userKeyWordServices;

        public UserKeyWordsApiController(AppDBContext appDb, IUserKeyWordServices userKeyWordServices)
        {
            _appDb = appDb;
            _userKeyWordServices = userKeyWordServices;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/UserKeyWords/GetMyUserKeyWords
        // يرجع Pivot Id + KeyWordsId (للتحديث/الحذف)
        // =========================================================
        [HttpGet("GetUserKeyWords")]
        public async Task<ActionResult<ApiResponse<object>>> GetMyUserKeyWords()
        {
            var userId = GetUserId();

            var list = await _appDb.UserKeyWords
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    userKeyWordId = x.Id,
                    keyWordsId = x.KeyWordsId
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User keywords loaded successfully.",
                Data = list
            });
        }

        // =========================================================
        // POST: api/UserKeyWords/CreateUserKeyWord
        // Body: { "keyWordsId": "GUID" }
        // =========================================================
        [HttpPost("CreateUserKeyWord")]
        public async Task<ActionResult<ApiResponse<object>>> CreateUserKeyWord([FromBody] UserKeyWordCreateDto req) // :contentReference[oaicite:10]{index=10}
        {
            var userId = GetUserId();
            req.UserId = userId;

            if (req.KeyWordsId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "keyWordsId is required."
                });
            }

            // ✅ تحقق أن KeyWords موجودة لتجنب FK 500
            var keywordExists = await _appDb.KeyWords
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(k => k.Id == req.KeyWordsId);

            if (!keywordExists)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Invalid keyWordsId: {req.KeyWordsId}"
                });
            }

            // منع التكرار
            var already = await _appDb.UserKeyWords
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.KeyWordsId == req.KeyWordsId);

            if (already)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Keyword already added for this user."
                });
            }

            var ok = await _userKeyWordServices.Create(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create user keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User keyword created successfully."
            });
        }

        // =========================================================
        // POST: api/UserKeyWords/CreateUserKeyWords
        // Body: { "keyWordsIds": ["GUID1","GUID2"] }
        // =========================================================
        [HttpPost("CreateUserKeyWords")]
        public async Task<ActionResult<ApiResponse<object>>> CreateUserKeyWords([FromBody] UserKeyWordsBulkCreateDto req)
        {
            var userId = GetUserId();

            if (req?.KeyWordsIds == null || req.KeyWordsIds.Count == 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "keyWordsIds is required."
                });
            }

            var ids = req.KeyWordsIds.Distinct().ToList();

            // ✅ تحقق من وجود كل الـ IDs في جدول KeyWords
            var existingKeywords = await _appDb.KeyWords
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(k => ids.Contains(k.Id))
                .Select(k => k.Id)
                .ToListAsync();

            var missingIds = ids.Except(existingKeywords).ToList();
            if (missingIds.Count > 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Some keyWordsIds do not exist.",
                    Data = new { missingIds }
                });
            }

            // الموجود مسبقاً للمستخدم
            var alreadyAdded = await _appDb.UserKeyWords
                .AsNoTracking()
                .Where(x => x.UserId == userId && ids.Contains(x.KeyWordsId))
                .Select(x => x.KeyWordsId)
                .ToListAsync();

            var toAdd = ids.Except(alreadyAdded).ToList();

            foreach (var keyWordsId in toAdd)
            {
                var ok = await _userKeyWordServices.Create(new UserKeyWordCreateDto
                {
                    UserId = userId,
                    KeyWordsId = keyWordsId
                });

                if (!ok)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Failed while adding keyWordsId: {keyWordsId}"
                    });
                }
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User keywords created successfully.",
                Data = new
                {
                    added = toAdd,
                    skipped = alreadyAdded
                }
            });
        }

        // =========================================================
        // PUT: api/UserKeyWords/UpdateUserKeyWord/{id}
        // Body: { "keyWordsId": "NEW-GUID" }
        // id = UserKeyWordId (pivot id)
        // =========================================================
        [HttpPut("UpdateUserKeyWord/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateUserKeyWord(Guid id, [FromBody] UserKeyWordUpdateDto req) // :contentReference[oaicite:11]{index=11}
        {
            var userId = GetUserId();

            if (req == null || req.KeyWordsId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "keyWordsId is required."
                });
            }

            var entity = await _appDb.UserKeyWords
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User keyword not found."
                });
            }

            var keywordExists = await _appDb.KeyWords
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(k => k.Id == req.KeyWordsId);

            if (!keywordExists)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Invalid keyWordsId: {req.KeyWordsId}"
                });
            }

            var already = await _appDb.UserKeyWords
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.KeyWordsId == req.KeyWordsId && x.Id != id);

            if (already)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Keyword already added for this user."
                });
            }

            req.Id = id;
            req.UserId = userId;

            var ok = await _userKeyWordServices.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update user keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User keyword updated successfully."
            });
        }

        // =========================================================
        // DELETE: api/UserKeyWords/DeleteUserKeyWord/{id}
        // =========================================================
        [HttpDelete("DeleteUserKeyWord/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUserKeyWord(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.UserKeyWords
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User keyword not found."
                });
            }

            var ok = await _userKeyWordServices.SoftDelete(id);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete user keyword."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User keyword deleted successfully."
            });
        }
    }
}
