using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GooMeppelUkraine.Web.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult Set(string culture, string? returnUrl = null)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}
