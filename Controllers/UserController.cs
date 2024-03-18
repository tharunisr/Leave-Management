using LeaveManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LeaveManagement.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register() 
        {
            List<UserTypeEntity> userTypes = GetUserTypes();
            ViewBag.UserTypes = userTypes;
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserEntity user)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (!IsUserAlreadyExists(user.Email))
                    {
                        UserTypeEntity userType = GetUserTypes()
                            .FirstOrDefault(ut => ut.UserTypeId == Convert.ToInt16(user.UserType));
                        user.UserType = userType.UserTypeName;
                        user.UserTypeId = userType.UserTypeId;
                        context.Users.Add(user);
                        context.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.UserAlreadyExistMsg = $"User already registered with {user.Email}";
                    }
                }
            }
            else
            {
                ModelState.AddModelError("register_error", "Invalid data");
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email,string password)
       {
 
            if(!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                if (IsUserAlreadyExists(email))
                {
                    UserEntity user=GetuserByEmail(email);
                    if (CheckPassword(user, password))
                    {
                        Session["loggedInUser"] = user;
                        Session["userTypeId"] = user.UserTypeId;
                        return RedirectToAction("Index", "Leave");
                    }
                    else
                    {
                        ViewBag.LoginDataRequired = $"Password not matched";
                    }
                }
            }
            else
            {
                ViewBag.LoginDataRequired = $"Email And Password Required For Login";
            }
            return View();
                

        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["loggedInUser"] = null;
            return View("login");
         
        }
        protected bool IsUserAlreadyExists(string email)
        {
            var result = context.Users.FirstOrDefault(u => u.Email.Equals(email));
            if (result != null)
            {
                return true;
            }
            return false;
        }

        protected List<UserTypeEntity> GetUserTypes()
        {
            return context.UserTypes.ToList();
        }
        protected UserEntity GetuserByEmail(string email)
        {
            if(!string.IsNullOrWhiteSpace(email))
            {
                return context.Users.FirstOrDefault(u=>u.Email.Equals(email));  
            }
            return null;
        }

        protected bool CheckPassword(UserEntity user, string password)
        {
            return password.Equals(user.Password);
        }
    }
}