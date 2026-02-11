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
    public class StatsManageController : Controller
    {
        private readonly ApplicationDbContext _db;
        public StatsManageController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var items = await _db.Stats.Where(x => x.Language == lang).OrderBy(x => x.Id).ToListAsync();
            ViewBag.Lang = lang;
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return View(new Stat { Language = lang });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Stat model)
        {
            if (!ModelState.IsValid) return View(model);
            
            _db.Stats.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Stats.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Stat model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = await _db.Stats.FindAsync(model.Id);
            if (item == null) return NotFound();

            item.Label = model.Label;
            item.Value = model.Value;
            item.Language = model.Language;

            _db.Stats.Update(item);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Stats.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Stats.FindAsync(id);
            if (item == null) return NotFound();

            _db.Stats.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
