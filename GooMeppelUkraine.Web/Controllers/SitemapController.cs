using System.Globalization;
using System.Text;
using GooMeppelUkraine.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SitemapController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("/sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var host = $"{Request.Scheme}://{Request.Host}";

            var urls = await _db.Articles
                .Where(a => a.IsPublished)
                .Select(a => new { a.Slug, a.Language, a.CreatedAtUtc })
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");

            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{host}/</loc>");
            sb.AppendLine("  </url>");

            foreach (var u in urls)
            {
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{host}/news/{u.Slug}</loc>");
                sb.AppendLine($"    <lastmod>{u.CreatedAtUtc:yyyy-MM-dd}</lastmod>");
                sb.AppendLine("  </url>");
            }

            sb.AppendLine("</urlset>");

            return Content(sb.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
