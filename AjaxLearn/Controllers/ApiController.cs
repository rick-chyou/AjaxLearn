using AjaxLearn.Models;
using Microsoft.AspNetCore.Mvc;

namespace AjaxLearn.Controllers
{
    public class ApiController : Controller
    {
        private readonly MyDBContext _context;
        public ApiController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(s => s.City).Distinct();
            return Json(cities);
        }
    }
}
