using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbWeb.Models
{
    public class BlogSummary
    {
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string ImageURL { get; set; }
    }
}
