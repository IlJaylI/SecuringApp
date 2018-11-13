using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common;

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
            try { 
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
            catch(CustomException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                //log message
                Logger.Log("guest", Request.Path, ex.Message);
                TempData["ErrorMessage"] = "Error has occured.We are investigating. Try again later...";
                return View();

            }
        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        { return View(); }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User u)
        {
            try
            {
                new UsersBL().Register(u);
                TempData["message"] = "Registeration succesful";
            }
            catch (CustomException ex)
            {
                TempData["errormessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Log("guest", Request.Path, ex
                    .Message);
                TempData["errormessage"] = "Registeration Failed";

            }

            return View(u);
        }
    }
}