using AjaxLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace AjaxLearn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JsonTest()
        {
            return View();
        }

        public IActionResult HW1()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult First()
        {
            return View();
        }

        public IActionResult Second()
        {
            return View();
        }

        public IActionResult HW2()
        {
            return View();
        }

        public IActionResult HW2Fetch()
        {
            return View();
        }


        public IActionResult Avatar()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }
        
        public IActionResult FormbyFetch()
        {
            return View();
        }

        public IActionResult FormbyFetchFormData()
        {
            return View();
        }

        public IActionResult JsontoApi()
        {
            return View();
        }
        
        public IActionResult apitest()
        {
            return View();
        }

        public IActionResult HW3()
        {
            return View();
        }

        public IActionResult HW4()
        {
            return View();
        }
    }
}
