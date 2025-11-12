using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AccountDTO
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public int AccountRole { get; set; }
        public string RoleName { get; set; } 
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
    public class AccountUpdateDto
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must not exceed 100 characters.")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Account role is required.")]
        [Range(1, 2, ErrorMessage = "Role must be 1 (Staff) or 2 (Lecturer).")]
        public int AccountRole { get; set; }
    }

    public class AccountCreateDto
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must not exceed 100 characters.")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string AccountPassword { get; set; }

        [Required(ErrorMessage = "Account role is required.")]
        [Range(1, 2, ErrorMessage = "Role must be 1 (Staff) or 2 (Lecturer).")]
        public int AccountRole { get; set; } 
    }
}
