using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class NewsStatisticDto
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedByAccountName { get; set; }
        public string CategoryName { get; set; }
    }
}
