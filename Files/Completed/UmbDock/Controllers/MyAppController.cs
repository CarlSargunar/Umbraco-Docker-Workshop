using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbDock.Controllers
{
    public class MyAppController : UmbracoApiController
    {
        public IEnumerable<string> GetBlogSummaries()
        {
            // Routes to /Umbraco/Api/MyApp/GetBlogSummaries
            return new[] { "Table", "Chair", "Desk", "Computer" };
        }
    }
}
