using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using alenMotorsWeb.Models;
using AlenMotorsDAL;

namespace alenMotorsWeb.Controllers {
    public class AccountController: Controller {
        // GET => /Account/Register 
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        // POST => /Account/Register 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            UserManagerResult registerResult = UserManager.Register(model.Email,
                                                                    model.FirstName,
                                                                    model.LastName,
                                                                    model.Password,
                                                                    model.Gender,
                                                                    model.PhoneNumber,
                                                                    model.BirthDate);
            if (registerResult.Success) {
                return RedirectToAction("Login", new LoginViewModel {Email = model.Email, Password = model.Password, RememberMe = false});
            }
            ModelState.AddModelError("", registerResult.ErrorMessage);
            return View(model);
        }

        // GET => /Account/Login 
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST => /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            UserManagerResult loginResult = UserManager.Login(model.Email, model.Password);
            if (loginResult.Success) {
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToAction("Account", "Account");
            }
            ModelState.AddModelError("", loginResult.ErrorMessage);
            return View(model);
            //if (model.Password == "b") {
            //    Roles.IsUserInRole("b", "admin");
            //    Roles.AddUserToRole("b", "admin");
            //    FormsAuthentication.SetAuthCookie("b", false);
            //    return RedirectToAction("ForgotPassword", "Account");
            //}

            //if (model.Password == "c") {
            //    Roles.IsUserInRole("b", "test");
            //    //Roles.AddUserToRole("c", "test1");
            //    FormsAuthentication.SetAuthCookie("c", false);
            //    //return RedirectToAction("Account", "Account");
            //}
            //return View();
        }


        // GET => /Account/ForgonPassword
        [AllowAnonymous]
        //[Authorize(Roles = "admin")]
        [Authorize]
        public ActionResult ForgotPassword() {
            return View();
        }


        // POST => /Account/ForgonPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model, string returlUrl) {
            if (model.Email == "a@a.com") {
                ModelState.AddModelError("", "Email was sent with your credentials info");
                return View(model);
            }
            return View();
        }


        // GET => /Account/Account
        [Authorize(Roles = "Developer")]
        public ActionResult Account() {
            return View();
        }

        //// Post => /Account/Account
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Account()
        //{
        //    return View();
        //}

        // GET => /Account/Logout
        public ActionResult LogOut() {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(UserManagerResult result) {
            ModelState.AddModelError("", result.ErrorMessage);
        }
    }
}