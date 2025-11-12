using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.DTO;

namespace NewsManagementSystem.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AuthController(AccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new
                {
                    message = "Email và mật khẩu là bắt buộc.",
                    statusCode = "400",
                    data = (object)null
                });
            }
            var loginResponse = await _accountService.Authenticate(loginDto);

            if (loginResponse == null)
            {
                return Unauthorized(new
                {
                    message = "Email hoặc mật khẩu không chính xác.",
                    statusCode = "401",
                    data = (object)null
                });
            }
            return Ok(new
            {
                message = "Đăng nhập thành công.",
                statusCode = "200",
                data = loginResponse 
            });
        }
    }
}
