using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GooMeppelUkraine.Web.Controllers;

[Authorize]
public class PrivateController : Controller
{
    public IActionResult Index()
    {
        return Content("OK: Identity + Authorization працює");
    }
}
