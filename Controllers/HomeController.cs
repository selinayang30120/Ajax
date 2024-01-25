using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication3.Models;
using WebApplication3.Models.DTO;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller

    {
        private readonly ILogger<HomeController> _logger;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

		[HttpPost]
		public IActionResult Index()
        {
            return View();
        }

		public IActionResult First()
		{
			return View();
		}

        public IActionResult Register(UserDTO _user)
        {
            return View();
        }

        public IActionResult HomeWork()
        {
            return View();
        }
        public IActionResult Homework2()
        {
            return View();
        }

        public IActionResult Homework3()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Spots()
        {
            return View();
        }

        public IActionResult AutoComplete()
        {
            return View();
        }


    }
}