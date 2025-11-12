using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class ProfileUpdateDto
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(100, ErrorMessage = "Account name must not exceed 100 characters.")]
        public string AccountName { get; set; }

    }
}
