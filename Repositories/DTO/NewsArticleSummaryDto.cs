using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class NewsArticleSummaryDto
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string NewsSource { get; set; }
        public string CategoryName { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
