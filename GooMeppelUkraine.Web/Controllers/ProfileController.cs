using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpGet("/profile")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var profile = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (profile == null)
            {
                profile = new UserProfile { UserId = user.Id };
                _db.UserProfiles.Add(profile);
                await _db.SaveChangesAsync();
            }

            ViewBag.Email = user.Email ?? "";
            return View(profile);
        }

        [HttpPost("/profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserProfile model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var profile = await _db.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (profile == null) return NotFound();

            profile.DisplayName = model.DisplayName?.Trim();

            await _db.SaveChangesAsync();

            TempData["Ok"] = "Profile updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}
