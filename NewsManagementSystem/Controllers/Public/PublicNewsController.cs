using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTO;
using Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Controllers.Public
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicNewsController : ControllerBase
    {
        private readonly INewsArticleService _newsService;
        public PublicNewsController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("news")]
        public async Task<IActionResult> GetAllActiveNews()
        {
            var newsList = await _newsService.GetAllActiveNewsAsync();

            var response = new ResponseDto<object>(
                message: "Lấy danh sách tin tức thành công.",
                statusCode: "200",
                data: newsList
            );
            return Ok(response);
        }

        [HttpGet("news/{id}")]
        public async Task<IActionResult> GetActiveNewsById(
            // Áp dụng validation cho tham số 'id'
            [Range(1, int.MaxValue, ErrorMessage = "ID tin tức không hợp lệ.")] int id)
        {
            var newsDetail = await _newsService.GetActiveNewsByIdAsync(id);

            if (newsDetail == null)
            {
                var notFoundResponse = new ResponseDto<object>(
                    message: "Không tìm thấy tin tức hoặc tin đã bị khóa.",
                    statusCode: "404",
                    data: null
                );
                return NotFound(notFoundResponse);
            }

            var successResponse = new ResponseDto<object>(
                message: "Lấy chi tiết tin tức thành công.",
                statusCode: "200",
                data: newsDetail
            );
            return Ok(successResponse);
        }
    }
}
