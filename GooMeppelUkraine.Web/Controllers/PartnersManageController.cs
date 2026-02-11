using System.Globalization;
using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Infrastructure;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
    public class PartnersManageController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PartnersManageController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var items = await _db.Partners.Where(x => x.Language == lang).OrderBy(x => x.Id).ToListAsync();
            ViewBag.Lang = lang;
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return View(new Partner { Language = lang });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Partners.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Partners.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Partner model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = await _db.Partners.FindAsync(model.Id);
            if (item == null) return NotFound();

            item.Name = model.Name;
            item.Url = model.Url;
            item.LogoUrl = model.LogoUrl;
            item.Language = model.Language;

            _db.Partners.Update(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Partners.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Partners.FindAsync(id);
            if (item == null) return NotFound();

            _db.Partners.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
