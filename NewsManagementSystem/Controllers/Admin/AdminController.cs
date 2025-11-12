using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.DTO;
using Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] string? search) 
        {
            var accounts = await _adminService.GetAccountsAsync(search);
            return Ok(new ResponseDto<object>("Accounts retrieved successfully.", "200", accounts));
        }

        [HttpGet("accounts/{id}")]
        public async Task<IActionResult> GetAccountById(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id)
        {
            var account = await _adminService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound(new ResponseDto<object>("Account not found.", "404", null));
            }
            return Ok(new ResponseDto<object>("Account retrieved successfully.", "200", account));
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDto dto)
        {
            var (createdAccount, error) = await _adminService.CreateAccountAsync(dto);

            if (error != null)
            {
                return BadRequest(new ResponseDto<object>(error, "400", null));
            }

         
            return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId },
                new ResponseDto<object>("Account created successfully.", "201", createdAccount));
        }

    
        [HttpPut("accounts/{id}")]
        public async Task<IActionResult> UpdateAccount(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id,
            [FromBody] AccountUpdateDto dto)
        {
            var (updatedAccount, error) = await _adminService.UpdateAccountAsync(id, dto);

            if (error != null)
            {
                return NotFound(new ResponseDto<object>(error, "404", null));
            }

            return Ok(new ResponseDto<object>("Account updated successfully.", "200", updatedAccount));
        }

     
        [HttpDelete("accounts/{id}")]
        public async Task<IActionResult> DeleteAccount(
            [Range(1, int.MaxValue, ErrorMessage = "Invalid ID.")] int id)
        {
            var (success, error) = await _adminService.DeleteAccountAsync(id);

            if (!success)
            {
                return BadRequest(new ResponseDto<object>(error, "400", null));
            }

            return Ok(new ResponseDto<object>("Account deleted successfully.", "200", null));
        }

      
        [HttpGet("reports/statistics")]
        public async Task<IActionResult> GetStatisticsReport(
            [FromQuery][Required] DateTime startDate,
            [FromQuery][Required] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new ResponseDto<object>("startDate cannot be later than endDate.", "400", null));
            }

            var report = await _adminService.GetStatisticsReportAsync(startDate, endDate);
            return Ok(new ResponseDto<object>("Statistics report retrieved successfully.", "200", report));
        }
    }
}
