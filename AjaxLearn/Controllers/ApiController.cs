using AjaxLearn.Models;
using AjaxLearn.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

            Thread.Sleep(50000);
            return Content("Hello Rick");
        }

        public IActionResult Register(string name, int age=28)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "guest";
            }
            return Content($"Hello{name},{age}歲了","text/plain", Encoding.UTF8);
        }

        public IActionResult Cities()
        {
            var cities = _context.Addresses.Select(s => s.City).Distinct();
            return Json(cities);
        }


        public IActionResult Avatar(int id = 1)
        {
            Member? member = _context.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                if (img != null)
                    return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult RegisterHW(UserDTO user)
        {
            //int a = 1;
            //int b = 0;
            //int c = a / b;
            //Thread.Sleep(2000);
            return Content((_context.Members.Any(m => m.Name == user.Name)).ToString());
        }
    }
}
