using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SecurityWebsite.Controllers
{
    public class AccountsController : Controller
    {
       [HttpGet]//getting /loading the page whether the user can input his email and password
       public ActionResult Login()
        {
            return View();
        }

        [HttpPost] //post the details submitted
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            UsersBL u = new UsersBL();
            if(u.Login(email,password))
            {
                //log in the user
                //formsAuthentication

                FormsAuthentication.SetAuthCookie(email, true);
                //populating a built-in object called Context.user with the email address specified
                //in the above line
                return RedirectToAction("Index", "Items");

            }
            else
            {
                TempData["ErrorMessage"] = "Login Failed";
                return View();
            }

        }
    }
}