using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UmbWeb.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace UmbWeb.Controllers
{
    public class MyAppController : UmbracoApiController
    {
        private readonly ILogger<MyAppController> _logger;
        private readonly IUmbracoContextFactory _contextFactory;

        public MyAppController(ILogger<MyAppController> logger,  IUmbracoContextFactory contextFactory)
        {
            _logger = logger;
            _contextFactory = contextFactory;
        }

        public IEnumerable<BlogSummary> GetBlogSummaries()
        {
            // Routes to /Umbraco/Api/MyApp/GetBlogSummaries
            // In a container, it runs from http://localhost:8000/Umbraco/Api/MyApp/GetBlogSummaries

            var blogSummaries = new List<BlogSummary>();

            using (var contextReference = _contextFactory.EnsureUmbracoContext())
            {
                IPublishedContentCache contentCache = contextReference.UmbracoContext.Content;
                IPublishedContent blogArticles = contentCache.GetAtRoot().FirstOrDefault().Children.FirstOrDefault(f => f.ContentType.Alias == "articleList");
                if (blogArticles != null)
                {
                    _logger.LogDebug("Blog list page found");
                    var articles = blogArticles.Children;
                    foreach (var article in articles)
                    {
                        if (article == null)
                        {
                            continue;
                        }

                        var blog = new BlogSummary()
                        {
                            Name = article.Name,
                            PublishDate = article.UpdateDate,
                            Title = article.Value<string>("title"),
                            Summary = article.Value<string>("subTitle"), //change the model name
                            AuthorId = article.CreatorId,
                            AuthorName = article.Value<string>("authorName"),
                            ImageURL = ""
                        };

                        blogSummaries.Add(blog);
                    }
                }
            }

            return blogSummaries;
        }

        public IEnumerable<string> GetBlogSummaries2()
        {
            
            // Routes to /Umbraco/Api/MyApp/GetBlogSummaries
            return new[] { "asdadwdw", "Chair", "Desk", "Computer" };
        }
    }
}
