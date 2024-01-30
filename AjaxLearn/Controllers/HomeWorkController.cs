using Microsoft.AspNetCore.Mvc;

namespace AjaxLearn.Controllers
{
    public class HomeWorkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
