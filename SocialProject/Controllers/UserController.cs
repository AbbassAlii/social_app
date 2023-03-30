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


    public UserController(ILogger<UserController> logger, ApplicationDbContext applicationDb)
    {
        _logger = logger;
        _applicationDb = applicationDb;
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
            public IActionResult Add(UserModel usr)
            {

                _applicationDb.Add(usr);
                _applicationDb.SaveChanges();


            return RedirectToActionPermanent("Index", "Home");
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

                


                return RedirectToActionPermanent("Index", "Home");
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
