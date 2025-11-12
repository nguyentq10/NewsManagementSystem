using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NewsManagementSystem.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles = "Staff")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        private int GetCurrentStaffId()
        {
            var staffIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(staffIdClaim, out int id))
            {
                return id;
            }
            return 0;
        }

        // === PROFILE ===

        // GET /api/staff/profile/me
        [HttpGet("profile/me")]
        public async Task<IActionResult> GetProfile()
        {
            var staffId = GetCurrentStaffId();
            if (staffId == 0)
            {
                return Unauthorized(new ResponseDto<object>("Invalid token.", "401", null));
            }

            var account = await _staffService.GetProfileAsync(staffId);
            if (account == null)
            {
                return NotFound(new ResponseDto<object>("Profile not found.", "404", null));
            }
            return Ok(new ResponseDto<object>("Profile retrieved successfully.", "200", account));
        }

        // PUT /api/staff/profile/me
        [HttpPut("profile/me")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto dto)
        {
            var staffId = GetCurrentStaffId();
            if (staffId == 0)
            {
                return Unauthorized(new ResponseDto<object>("Invalid token.", "401", null));
            }

            var (account, error) = await _staffService.UpdateProfileAsync(staffId, dto);
            if (error != null)
            {
                return NotFound(new ResponseDto<object>(error, "404", null));
            }
            return Ok(new ResponseDto<object>("Profile updated successfully.", "200", account));
        }

        // === CATEGORY ===

        // GET /api/staff/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _staffService.GetCategoriesAsync();
            return Ok(new ResponseDto<object>("Categories retrieved successfully.", "200", categories));
        }

        // POST /api/staff/categories
        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateUpdateDto dto)
        {
            var (category, error) = await _staffService.CreateCategoryAsync(dto);
            if (error != null)
            {
                return BadRequest(new ResponseDto<object>(error, "400", null));
            }

            // Trả về 201 Created
            return CreatedAtAction(nameof(GetCategories), new { id = category.CategoryId },
                new ResponseDto<object>("Category created successfully.", "201", category));
        }

        // PUT /api/staff/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id,
            [FromBody] CategoryCreateUpdateDto dto)
        {
            var (category, error) = await _staffService.UpdateCategoryAsync(id, dto);
            if (error != null)
            {
                return NotFound(new ResponseDto<object>(error, "404", null));
            }
            return Ok(new ResponseDto<object>("Category updated successfully.", "200", category));
        }

        // DELETE /api/staff/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id)
        {
            var (success, error) = await _staffService.DeleteCategoryAsync(id);
            if (!success)
            {
                // Lỗi có thể là 400 (giàng buộc) hoặc 404 (không tìm thấy)
                return BadRequest(new ResponseDto<object>(error, "400", null));
            }
            return Ok(new ResponseDto<object>("Category deleted successfully.", "200", null));
        }

        // === NEWS ARTICLE ===

        // GET /api/staff/news
        [HttpGet("news")]
        public async Task<IActionResult> GetAllNews()
        {
            var staffId = GetCurrentStaffId();
            var newsList = await _staffService.GetNewsAsync(staffId, historyOnly: false);
            return Ok(new ResponseDto<object>("News list retrieved successfully.", "200", newsList));
        }

        // GET /api/staff/news/history
        [HttpGet("news/history")]
        public async Task<IActionResult> GetNewsHistory()
        {
            var staffId = GetCurrentStaffId();
            if (staffId == 0)
            {
                return Unauthorized(new ResponseDto<object>("Invalid token.", "401", null));
            }

            var newsList = await _staffService.GetNewsAsync(staffId, historyOnly: true);
            return Ok(new ResponseDto<object>("News history retrieved successfully.", "200", newsList));
        }

        // POST /api/staff/news
        [HttpPost("news")]
        public async Task<IActionResult> CreateNews([FromBody] NewsArticleCreateUpdateDto dto)
        {
            var staffId = GetCurrentStaffId();
            if (staffId == 0)
            {
                return Unauthorized(new ResponseDto<object>("Invalid token.", "401", null));
            }

            var (news, error) = await _staffService.CreateNewsAsync(dto, staffId);
            if (error != null)
            {
                return BadRequest(new ResponseDto<object>(error, "400", null));
            }

            return CreatedAtAction(nameof(GetAllNews), new { id = news.NewsArticleId },
                new ResponseDto<object>("News article created successfully.", "201", news));
        }

        // PUT /api/staff/news/{id}
        [HttpPut("news/{id}")]
        public async Task<IActionResult> UpdateNews(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id,
            [FromBody] NewsArticleCreateUpdateDto dto)
        {
            var staffId = GetCurrentStaffId();
            if (staffId == 0)
            {
                return Unauthorized(new ResponseDto<object>("Invalid token.", "401", null));
            }

            var (news, error) = await _staffService.UpdateNewsAsync(id, dto, staffId);
            if (error != null)
            {
                return NotFound(new ResponseDto<object>(error, "404", null));
            }
            return Ok(new ResponseDto<object>("News article updated successfully.", "200", news));
        }

        // DELETE /api/staff/news/{id}
        [HttpDelete("news/{id}")]
        public async Task<IActionResult> DeleteNews(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id)
        {
            var (success, error) = await _staffService.DeleteNewsAsync(id);
            if (!success)
            {
                return NotFound(new ResponseDto<object>(error, "404", null));
            }
            return Ok(new ResponseDto<object>("News article deleted successfully.", "200", null));
        }
    }
}
