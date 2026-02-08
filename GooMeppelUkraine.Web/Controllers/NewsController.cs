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

        // /News
        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var items = await _db.Articles
                .Where(a => a.IsPublished && a.Language == lang)
                .OrderByDescending(a => a.PublishedAtUtc ?? a.CreatedAtUtc)
                .ToListAsync();

            return View(items);
        }

        // /News/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var item = await _db.Articles.FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.IsPublished &&
                a.Language == lang
            );

            if (item == null)
                return NotFound();

            return View(item);
        }
    }
}
