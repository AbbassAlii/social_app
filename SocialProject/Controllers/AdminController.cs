using SocialProject.Controllers;
using SocialProject.Data;
using SocialProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SocialProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _applicationDb;

        public AdminController(ApplicationDbContext applicationDb)
        {

            _applicationDb = applicationDb;
        }





        public IActionResult Signup()
        {



            return View();
        }
        [HttpPost]
        public IActionResult Signup(Admin usr)
        {

            _applicationDb.Add(usr);
            _applicationDb.SaveChanges();


            return RedirectToActionPermanent("login", "Admin");
        }


        [HttpGet]
        public IActionResult login()
        {
            Admin _adminModel = new Admin();
            return View(_adminModel);
        }
        [HttpPost]
        public IActionResult login(Admin _adminModel)
        {
            // int UserIdd = Convert.ToInt32(contextAccessor.HttpContext.Session[UseId]);

            var Found = _applicationDb.Admins.Where(m => m.Name == _adminModel.Name && m.Password == _adminModel.Password).FirstOrDefault();


            if (Found == null)
            {
                ViewBag.LoginStatus = 0;
            }
            else
            {

                HttpContext.Session.SetString("UserName", Found.Name);
                HttpContext.Session.SetString("Password", Found.Password);

                return RedirectToActionPermanent("Index", "Admin");
            }

            return View(_adminModel);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Addclass()
        {
            return View();
        }
        public IActionResult Allclass()
        {
            return View();
        }
        public IActionResult Addstudent()
        {
            return View();
        }
        public IActionResult Studentdetails()
        {
            return View();
        }
        public IActionResult Admitform()
        {
            return View();
        }
        public IActionResult Allstudent()
        {
            return View();
        }

        public IActionResult Allteacher()
        {
            return View();
        }
        public IActionResult Addteacher()
        {
            return View();
        }

        public IActionResult Classroutine()
        {
            return View();
        }

        public IActionResult Teacherdetails()
        {
            return View();
        }

    }
}
