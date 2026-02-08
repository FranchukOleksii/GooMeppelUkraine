using System.Diagnostics;
using System.Globalization;
using GooMeppelUkraine.Web.Contexts;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var vm = new HomeVm
            {
                Stats = await _db.Stats.Where(x => x.Language == lang).ToListAsync(),
                Team = await _db.TeamMembers.Where(x => x.Language == lang).ToListAsync(),
                Partners = await _db.Partners.Where(x => x.Language == lang).ToListAsync(),
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
