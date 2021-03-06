﻿using System.Web.Mvc;
using System.Web.Security;
using alenMotorsWeb.Models;
using AlenMotorsDAL;

namespace alenMotorsWeb.Controllers {
    public class AccountController: Controller {
        // GET => /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            if (User.Identity.IsAuthenticated) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST => /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            UserManagerResult registerResult = UserManager.AddUser(model.Email,
                                                                   model.FirstName,
                                                                   model.LastName,
                                                                   model.Password,
                                                                   model.Gender,
                                                                   model.PhoneNumber,
                                                                   model.BirthDate);
            if (registerResult.Success) {
                //return RedirectToAction("Login", "Account");

                System.Net.Mail.MailMessage mailMessage =
                new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress("sender@mydomain.com", "Alen Motors Registration"),
                                                new System.Net.Mail.MailAddress(model.Email));
                mailMessage.Subject = "Email confirmation";
                mailMessage.Body =
                string.Format(
                              "Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>",
                              model.FirstName + " " + model.LastName,
                              Url.Action("ConfirmEmail",
                                         "Account",
                                         new {Email = model.Email, verifyString = registerResult.VerifyString},
                                         Request.Url.Scheme));
                mailMessage.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("asslister@gmail.com", "zkwsbhirtimywuwt");
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
                TempData["RegisterError"] = "Registration complete, please check your email for confirmation";
                return RedirectToAction("Login", "Account" /*, new {Email = model.Email}*/);
            }
            ModelState.AddModelError("", registerResult.ErrorMessage);
            return View(model);
        }   

        // GET => /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            if (TempData["RegisterError"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RegisterError"].ToString());
            }
            if (TempData["VerifyError"] != null) {
                ModelState.AddModelError(string.Empty, TempData["VerifyError"].ToString());
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST => /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            UserManagerResult loginResult = UserManager.Login(model.Email, model.Password);
            if (loginResult.Success) {
                FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                if (returnUrl != null) {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Account", "Account");
            }
            ModelState.AddModelError("", loginResult.ErrorMessage);
            return View(model);
        }

        // GET => /Account/ForgonPassword
        [AllowAnonymous]
        [Authorize]
        public ActionResult ForgotPassword() {
            return View();
        }

        // POST => /Account/ForgonPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model, string returlUrl) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            if (model.Email == "a@a.com") {
                ModelState.AddModelError("", "Email was sent with your credentials info");
                return View(model);
            }
            return View();
        }

        // GET => /Account/Account
        [Authorize(Roles = "Developer, User, Employee, Manager")]
        public ActionResult Account() {
            //if (Roles.IsUserInRole("Developer")) {
            //    return RedirectToAction("Index", "Home");
            //}
            UserManagerResult getUserResult = UserManager.GetUser(User.Identity.Name);
            AccountViewModel model = new AccountViewModel();
            if (!getUserResult.Success) {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
            model.LastName = getUserResult.User.LastName.Replace(" ", string.Empty);
            model.FirstName = getUserResult.User.FirstName.Replace(" ", string.Empty);
            model.BirthDate = getUserResult.User.BirthDate.Replace(" ", string.Empty);
            model.Gender = getUserResult.User.Gender.Replace(" ", string.Empty);
            model.PhoneNumber = getUserResult.User.PhoneNumber;
            return View(model);
        }

        // Post => /Account/Account
        [HttpPost]
        [Authorize(Roles = "Developer, User, Employee, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Account(AccountViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            Account userAccount = new Account {
                Email = User.Identity.Name,
                LastName = model.LastName,
                FirstName = model.FirstName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
            };
            UserManagerResult updateUserResult = UserManager.EditUser(User.Identity.Name, userAccount, model.NewPassword);
            if (updateUserResult.ErrorMessage != null) {
                ModelState.AddModelError("", updateUserResult.ErrorMessage);
                model.Password = "";
                model.NewPassword = "";
                model.ConfirmNewPassword = "";
                return View(model);
            }
            if (updateUserResult.Success) {
                ModelState.AddModelError("", "Changes Saved");
                model.Password = "";
                model.NewPassword = "";
                model.ConfirmNewPassword = "";
                return View(model);
            }
            ModelState.AddModelError("", "The old password you entered is wrong");
            model.Password = "";
            model.NewPassword = "";
            model.ConfirmNewPassword = "";
            return View(model);
        }

        // GET => /Account/Logout
        public ActionResult LogOut() {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail(string email, string verifyString) {
            UserManagerResult userManagerResult = UserManager.VerifyUser(email, verifyString);
            if (!userManagerResult.Success) {
                TempData["VerifyError"] = userManagerResult.ErrorMessage;
                return RedirectToAction("Login", "Account");
            }
            TempData["VerifyError"] = "Account verified successfully";
            return RedirectToAction("Login", "Account");
        }
    }
}