using GooMeppelUkraine.Web.Infrastructure;
using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateUser()
        {
            return View(new AdminCreateUserVm { Role = Roles.User });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(AdminCreateUserVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            if (!await _roleManager.RoleExistsAsync(vm.Role))
            {
                ModelState.AddModelError(nameof(vm.Role), "Role does not exist.");
                return View(vm);
            }

            var existing = await _userManager.FindByEmailAsync(vm.Email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(vm.Email), "User already exists.");
                return View(vm);
            }

            var user = new IdentityUser
            {
                UserName = vm.Email,
                Email = vm.Email,
                EmailConfirmed = true
            };

            var create = await _userManager.CreateAsync(user, vm.Password);
            if (!create.Succeeded)
            {
                foreach (var e in create.Errors)
                    ModelState.AddModelError("", $"{e.Code}: {e.Description}");
                return View(vm);
            }

            var addRole = await _userManager.AddToRoleAsync(user, vm.Role);
            if (!addRole.Succeeded)
            {
                foreach (var e in addRole.Errors)
                    ModelState.AddModelError("", $"{e.Code}: {e.Description}");
                return View(vm);
            }

            TempData["Success"] = $"User created: {vm.Email} ({vm.Role})";
            return RedirectToAction(nameof(CreateUser));
        }
    }
}