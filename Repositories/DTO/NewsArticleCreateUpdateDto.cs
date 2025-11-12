using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class NewsArticleCreateUpdateDto
    {
        [Required(ErrorMessage = "News title is required.")]
        [StringLength(255, ErrorMessage = "Title must not exceed 255 characters.")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required.")]
        [StringLength(500, ErrorMessage = "Headline must not exceed 500 characters.")]
        public string Headline { get; set; }

        [Required(ErrorMessage = "News content is required.")]
        public string NewsContent { get; set; }

        [StringLength(255, ErrorMessage = "News source must not exceed 255 characters.")]
        public string NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid CategoryID.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "News status is required.")]
        public bool NewsStatus { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
