using SocialProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SocialProject.Controllers
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
		public IActionResult Setting()
		{
			return View();
		}
		public IActionResult Blogs()
		{
			return View();
		}
		public IActionResult Events()
		{
			return View();
		}
		public IActionResult Notifications()
		{
			return View();
		}

		public IActionResult MyProfile()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult FAQs()
        {

              return View();
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}