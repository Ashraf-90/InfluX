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
    [Route("api/UserNiches")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserNicheApiController : ControllerBase
    {
        private readonly AppDBContext _appDb;
        private readonly IUserNicheServices _userNicheServices;

        public UserNicheApiController(AppDBContext appDb, IUserNicheServices userNicheServices)
        {
            _appDb = appDb;
            _userNicheServices = userNicheServices;
        }

        private Guid GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(idStr!);
        }

        // =========================================================
        // GET: api/UserNiches/GetMyUserNiches
        // يرجع UserNicheId + NicheId (مفيد للحذف والتحديث)
        // =========================================================
        [HttpGet("GetUserNiches")]
        public async Task<ActionResult<ApiResponse<object>>> GetMyUserNiches()
        {
            var userId = GetUserId();

            var list = await _appDb.UserNiches
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    userNicheId = x.Id,
                    nicheId = x.NicheId
                })
                .ToListAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User niches loaded successfully.",
                Data = list
            });
        }

        // =========================================================
        // POST: api/UserNiches/CreateUserNiche
        // Body: { "nicheId": "GUID" }
        // =========================================================
        [HttpPost("CreateUserNiche")]
        public async Task<ActionResult<ApiResponse<object>>> CreateUserNiche([FromBody] UserNicheCreateDto req)
        {
            var userId = GetUserId();
            req.UserId = userId;

            // ✅ تحقق أن الـ Niche موجود لتجنب FK 500
            var nicheExists = await _appDb.Niches
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(n => n.Id == req.NicheId);

            if (!nicheExists)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Invalid nicheId: {req.NicheId}"
                });
            }

            // منع التكرار
            var already = await _appDb.UserNiches
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.NicheId == req.NicheId);

            if (already)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Niche already added for this user."
                });
            }

            var ok = await _userNicheServices.Create(req);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to create user niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User niche created successfully."
            });
        }

        // =========================================================
        // POST: api/UserNiches/CreateUserNiches
        // Body:
        // {
        //   "nicheIds": ["GUID1","GUID2"]
        // }
        // =========================================================
        [HttpPost("CreateUserNiches")]
        public async Task<ActionResult<ApiResponse<object>>> CreateUserNiches([FromBody] UserNichesBulkCreateDto req)
        {
            var userId = GetUserId();

            if (req?.NicheIds == null || req.NicheIds.Count == 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "nicheIds is required."
                });
            }

            var nicheIds = req.NicheIds.Distinct().ToList();

            // تحقق أن كل الـ IDs موجودة في Niches
            var existingNiches = await _appDb.Niches
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(n => nicheIds.Contains(n.Id))
                .Select(n => n.Id)
                .ToListAsync();

            var missingIds = nicheIds.Except(existingNiches).ToList();

            if (missingIds.Count > 0)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Some nicheIds do not exist.",
                    Data = new { missingIds }
                });
            }

            // الموجود مسبقاً للمستخدم
            var alreadyAdded = await _appDb.UserNiches
                .AsNoTracking()
                .Where(x => x.UserId == userId && nicheIds.Contains(x.NicheId))
                .Select(x => x.NicheId)
                .ToListAsync();

            var toAdd = nicheIds.Except(alreadyAdded).ToList();

            foreach (var nicheId in toAdd)
            {
                var ok = await _userNicheServices.Create(new UserNicheCreateDto
                {
                    UserId = userId,
                    NicheId = nicheId
                });

                if (!ok)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Failed while adding nicheId: {nicheId}"
                    });
                }
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User niches created successfully.",
                Data = new
                {
                    added = toAdd,
                    skipped = alreadyAdded
                }
            });
        }


        // =========================================================
        // PUT: api/UserNiches/UpdateUserNiche/{id}
        // Body: { "nicheId": "NEW-NICHE-GUID" }
        // id = UserNicheId (ليس UserId)
        // =========================================================
        [HttpPut("UpdateUserNiche/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateUserNiche(Guid id, [FromBody] UserNicheUpdateDto req)
        {
            var userId = GetUserId();

            if (req == null || req.NicheId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "nicheId is required."
                });
            }

            // Ownership check
            var entity = await _appDb.UserNiches
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User niche not found."
                });
            }

            // تأكد أن الـ NicheId الجديد موجود
            var nicheExists = await _appDb.Niches
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(n => n.Id == req.NicheId);

            if (!nicheExists)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Invalid nicheId: {req.NicheId}"
                });
            }

            // منع التكرار (نفس المستخدم + نفس الـ niche)
            var already = await _appDb.UserNiches
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.NicheId == req.NicheId && x.Id != id);

            if (already)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Niche already added for this user."
                });
            }

            // Force ids
            req.Id = id;
            req.UserId = userId;

            var ok = await _userNicheServices.Update(req);
            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to update user niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User niche updated successfully."
            });
        }


        // =========================================================
        // DELETE: api/UserNiches/DeleteUserNiche/{id}
        // =========================================================
        [HttpDelete("DeleteUserNiche/{id:guid}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUserNiche(Guid id)
        {
            var userId = GetUserId();

            var entity = await _appDb.UserNiches
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (entity == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User niche not found."
                });
            }

            var ok = await _userNicheServices.SoftDelete(id);

            if (!ok)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Failed to delete user niche."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User niche deleted successfully."
            });
        }
    }
}
