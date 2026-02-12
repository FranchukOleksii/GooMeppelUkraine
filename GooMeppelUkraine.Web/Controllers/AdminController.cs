using GooMeppelUkraine.Web.Infrastructure;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Controllers
{ 

    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateUser() => View(new AdminCreateUserVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(AdminCreateUserVm model)
        {
            if (!ModelState.IsValid) return View(model);

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError(nameof(model.Role), "Role does not exist.");
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            TempData["Ok"] = "User created.";
            return RedirectToAction(nameof(CreateUser));
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();

            var model = new List<AdminUserVm>();

            foreach (var user in users)
            {
                model.Add(new AdminUserVm
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            return View(new AdminUserVm
            {
                Id = user.Id,
                Email = user.Email ?? "",
                Roles = roles
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!string.IsNullOrWhiteSpace(role))
                await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(Users));
        }

    }
}