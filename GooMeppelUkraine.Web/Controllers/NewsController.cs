using System.Globalization;
using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GooMeppelUkraine.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public NewsController(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var cacheKey = $"news_index_{lang}";

            var items = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                return await _db.Articles
                    .Where(a => a.IsPublished && a.Language == lang)
                    .OrderByDescending(a => a.CreatedAtUtc)
                    .ToListAsync();
            });

            return View(items);
        }

        [HttpGet("/news/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var cacheKey = $"news_details_{lang}_{slug}";

            var article = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                return await _db.Articles.FirstOrDefaultAsync(a =>
                    a.IsPublished &&
                    a.Language == lang &&
                    a.Slug == slug
                );
            });

            if (article == null) return NotFound();

            ViewData["MetaTitle"] = string.IsNullOrWhiteSpace(article.MetaTitle) ? article.Title : article.MetaTitle;
            ViewData["MetaDescription"] = article.MetaDescription;

            return View(article);
        }
    }
}
