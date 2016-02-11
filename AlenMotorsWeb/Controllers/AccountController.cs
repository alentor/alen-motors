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
            if (model.Email == "a@a.com") {
                ModelState.AddModelError("", "Email was sent with your credentials info");
                return View(model);
            }
            return View();
        }

        // GET => /Account/Account
        [Authorize(Roles = "Developer")]
        public ActionResult Account() {
            AccountViewModels model = new AccountViewModels();
            UserManagerResult getUserResult = UserManager.GetUserInformation(User.Identity.Name);
            model.LastName = getUserResult.User.LastName.Replace(" ", string.Empty);
            model.FirstName = getUserResult.User.FirstName.Replace(" ", string.Empty);
            model.BirthDate = getUserResult.User.BirthDate.Replace(" ", string.Empty);
            model.Gender = getUserResult.User.Gender.Replace(" ", string.Empty);
            model.PhoneNumber = getUserResult.User.PhoneNumber;
            return View(model);
        }

        // Post => /Account/Account
        [HttpPost]
        [Authorize(Roles = "Developer")]
        [ValidateAntiForgeryToken]
        public ActionResult Account(AccountViewModels model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            Account userAccount = new Account {
                LastName = model.LastName,
                FirstName = model.FirstName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                // We are using registation date to pass the new password
                RegistrationDate = model.NewPassword
            };
            UserManagerResult updateUserResult = UserManager.UpdateUser(User.Identity.Name, userAccount);
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
    }
}