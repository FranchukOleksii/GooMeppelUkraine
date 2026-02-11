using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Infrastructure;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace GooMeppelUkraine.Web.Controllers 
{ 

    [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
    public class NewsManageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;
        private readonly SlugService _slugService;

        public NewsManageController(ApplicationDbContext db, IMemoryCache cache, SlugService slugService)
        {
            _db = db;
            _cache = cache;
            _slugService = slugService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _db.Articles
                .OrderByDescending(a => a.CreatedAtUtc)
                .ToListAsync();

            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return View(new Article { Language = lang });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article model)
        {
            model.Slug = await _slugService.GenerateUniqueSlugAsync(model.Title, model.Language);
            ModelState.Remove(nameof(Article.Slug));

            if (!ModelState.IsValid) 
                return View(model);

            model.CreatedAtUtc = DateTime.UtcNow;

            _db.Articles.Add(model);
            await _db.SaveChangesAsync();

            InvalidateNewsCache(model.Language, model.Slug);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Article model)
        {
            model.Slug = SlugHelper.Generate(model.Title);
            ModelState.Remove(nameof(Article.Slug));

            if (!ModelState.IsValid) 
                return View(model);

            var item = await _db.Articles.FindAsync(model.Id);
            if (item == null) 
                return NotFound();

            item.Title = model.Title;
            item.Content = model.Content;
            item.Language = model.Language;
            item.IsPublished = model.IsPublished;
            
            item.MetaTitle = model.MetaTitle;
            item.MetaDescription = model.MetaDescription;

            item.Slug = await _slugService.GenerateUniqueSlugAsync(item.Title, item.Language, excludeArticleId: item.Id);

            await _db.SaveChangesAsync();

            InvalidateNewsCache(item.Language, item.Slug);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            _db.Articles.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Publish(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            if (!item.IsPublished)
            {
                item.IsPublished = true;
                item.PublishedAtUtc = DateTime.UtcNow;
                await _db.SaveChangesAsync();

                InvalidateNewsCache(item.Language, item.Slug);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unpublish(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            if (item.IsPublished)
            {
                item.IsPublished = false;
                item.PublishedAtUtc = null;
                await _db.SaveChangesAsync();

                InvalidateNewsCache(item.Language, item.Slug);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            var article = await _db.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (article == null) return NotFound();

            ViewData["MetaTitle"] = string.IsNullOrWhiteSpace(article.MetaTitle) ? article.Title : article.MetaTitle;
            ViewData["MetaDescription"] = article.MetaDescription;

            return View("~/Views/News/Details.cshtml", article);
        }

        private void InvalidateNewsCache(string lang, string? slug = null)
        {
            _cache.Remove($"news_index_{lang}");
            if (!string.IsNullOrWhiteSpace(slug))
                _cache.Remove($"news_details_{lang}_{slug}");
        }
    }
}