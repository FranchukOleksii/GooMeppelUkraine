using System.Globalization;
using GooMeppelUkraine.Web.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NewsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var items = await _db.Articles
                .Where(a => a.IsPublished && a.Language == lang)
                .OrderByDescending(a => a.PublishedAtUtc ?? a.CreatedAtUtc)
                .ToListAsync();

            return View(items);
        }

        [HttpGet("/news/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var article = await _db.Articles.FirstOrDefaultAsync(a =>
                a.IsPublished &&
                a.Language == lang &&
                a.Slug == slug
            );

            if (article == null) return NotFound();

            ViewData["MetaTitle"] = string.IsNullOrWhiteSpace(article.MetaTitle) ? article.Title : article.MetaTitle;
            ViewData["MetaDescription"] = article.MetaDescription;

            return View(article);
        }
    }
}
