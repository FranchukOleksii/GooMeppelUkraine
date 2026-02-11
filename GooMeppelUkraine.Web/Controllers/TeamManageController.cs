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
    public class TeamManageController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TeamManageController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var items = await _db.TeamMembers.Where(x => x.Language == lang).OrderBy(x => x.Id).ToListAsync();
            ViewBag.Lang = lang;
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            return View(new TeamMember { Language = lang });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMember model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.TeamMembers.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.TeamMembers.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TeamMember model)
        {
            if (!ModelState.IsValid) return View(model);

            var item = await _db.TeamMembers.FindAsync(model.Id);
            if (item == null) return NotFound();

            item.Name = model.Name;
            item.Role = model.Role;
            item.PhotoUrl = model.PhotoUrl;
            item.Language = model.Language;

            _db.TeamMembers.Update(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.TeamMembers.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.TeamMembers.FindAsync(id);
            if (item == null) return NotFound();

            _db.TeamMembers.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
