using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Infrastructure;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GooMeppelUkraine.Web.Controllers 
{ 

    [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
    public class NewsManageController : Controller
    {
        private readonly ApplicationDbContext _db;

        public NewsManageController(ApplicationDbContext db)
        {
            _db = db;
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
        public async Task<IActionResult> Create(Article model)
        {
            if (!ModelState.IsValid) return View(model);

            model.CreatedAtUtc = DateTime.UtcNow;
            _db.Articles.Add(model);
            await _db.SaveChangesAsync();

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
        public async Task<IActionResult> Edit(Article model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = await _db.Articles.FindAsync(model.Id);
            if (item == null) return NotFound();

            item.Title = model.Title;
            item.Content = model.Content;
            item.Language = model.Language;
            item.IsPublished = model.IsPublished;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            _db.Articles.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Publish(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            if (!item.IsPublished)
            {
                item.IsPublished = true;
                item.PublishedAtUtc = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Unpublish(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            if (item.IsPublished)
            {
                item.IsPublished = false;
                item.PublishedAtUtc = null;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _db.Articles.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }
    }
}