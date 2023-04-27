using SocialProject.Data;
using SocialProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SocialProject.Controllers
{
    public class UserController : Controller
    { 
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _applicationDb;
		private readonly IWebHostEnvironment _environment;

		public UserController(ILogger<UserController> logger, IWebHostEnvironment environment, ApplicationDbContext applicationDb)
    {
        _logger = logger;
        _applicationDb = applicationDb;
			_environment = environment;
		}
    
        public IActionResult Index()
        {
            return View();
        }
       
        public IActionResult Add()
        {
            


                return View();
            }
            [HttpPost]
		public async Task<IActionResult> Add(UserModel usr, IFormFile file)
            {
			
            if (file != null && file.Length > 0)
			{
				// save file to the server
				var uploads = Path.Combine(_environment.WebRootPath, "uploads");
				if (!Directory.Exists(uploads))
				{
					Directory.CreateDirectory(uploads);
				}

				var fileName = Path.GetFileName(file.FileName);
				using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
					usr.Attachment =  fileName;
				}
			}
			_applicationDb.Add(usr);
                _applicationDb.SaveChanges();


            return RedirectToActionPermanent("login", "User");
        }


        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(UserModel usr)
        {


            
            var Found = _applicationDb.UserModels.Where(m => m.Email == usr.Email && m.Password == usr.Password).FirstOrDefault();


            
            if (Found == null)
            {
                ViewBag.LoginStatus = 0;
            }
            else
            {

                HttpContext.Session.SetString("UserName", Found.UserName);
                HttpContext.Session.SetString("Password", Found.Password);
                HttpContext.Session.SetString("Attachment", "/Uploads/"+Found.Attachment);
				HttpContext.Session.SetInt32("UserId", Found.UserId);




				return RedirectToActionPermanent("Create", "PostModels");
            }
            return View(usr);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }


    }
}
