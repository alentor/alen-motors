using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alenMotorsWeb.Models;
using AlenMotorsDAL;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;


namespace alenMotorsWeb.Controllers {
    public class ManagementController: Controller {
        // GET => Management/Developer
        [Authorize(Roles = "Employee")]
        public ActionResult Manage(ManagementViewModel model, int? page) {
            // Error Filtering
            if (TempData["RoleAdded"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RoleAdded"].ToString());
            }
            if (TempData["RoleRemoved"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RoleRemoved"].ToString());
            }
            if (TempData["UserRemoved"] != null) {
                ModelState.AddModelError(string.Empty, TempData["UserRemoved"].ToString());
            }
            if (TempData["ManagementError"] != null) {
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
            }
            if (TempData["UserUpdated"] != null) {
                ModelState.AddModelError(string.Empty, TempData["UserUpdated"].ToString());
            }
            if (TempData["EditRole"] != null) {
                ModelState.AddModelError(string.Empty, TempData["EditRole"].ToString());
            }
            // Roels for DropDown
            UserRoleManagerResult getRoleListResult = UserRoleManager.GetRoles();
            if (getRoleListResult.Success) {
                model.RoleList = getRoleListResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            }
            else {
                TempData["ManagementError"] = getRoleListResult.ErrorMessage;
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
            }

            // Uset List
            UserManagerResult getUserListResult = UserManager.GetUsers();
            if (getUserListResult.Success) {
                model.UserList = getUserListResult.UserList.ToList();
                foreach (Account account in getUserListResult.UserList) {
                    UserRoleManagerResult getUserRoles = UserRoleManager.GetUserRoles(account.Email.Replace(" ", String.Empty));
                    string roles = null;
                    // remove this IF when ROLES fully implanted
                    if (getUserRoles != null) {
                        roles = getUserRoles.Roles.Aggregate(roles, (current, role) => current + role + " ");
                    }
                    ManagementAccountDisplayModelView accountModel = new ManagementAccountDisplayModelView {
                        Email = account.Email.Replace(" ", String.Empty),
                        LastName = account.LastName.Replace(" ", String.Empty),
                        FirstName = account.FirstName.Replace(" ", String.Empty),
                        Gender = account.Gender.Replace(" ", String.Empty),
                        BirthDate = account.BirthDate.Replace(" ", String.Empty),
                        PhoneNumber = account.PhoneNumber,
                        Roles = roles
                    };
                    model.AccountModels.Add(accountModel);
                }
            }
            else {
                TempData["ManagementError"] = getUserListResult.ErrorMessage;
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
            }

            return View(model);
        }

        // Post => Management/AddNewRole
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult AddNewRole(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Manage", "Management");
            }
            UserRoleManagerResult addNewRole = UserRoleManager.AddRole(model.NewRole);
            if (addNewRole.Success) {
                TempData["RoleAdded"] = "Roled Added";
            }
            else {
                TempData["RoleAdded"] = addNewRole.ErrorMessage;
                return RedirectToAction("Manage", "Management");
            }
            return RedirectToAction("Manage", "Management");
        }

        // Post => Management/RemoveRole
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult RemvoeRole(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Manage", "Management");
            }
            UserRoleManagerResult removeRole = UserRoleManager.RemoveRole(model.RoleToRemove);
            if (removeRole.Success) {
                TempData["RoleRemoved"] = "Role Removed";
            }
            else {
                TempData["RoleRemoved"] = removeRole.ErrorMessage;
                return RedirectToAction("Manage", "Management");
            }
            return RedirectToAction("Manage", "Management");
        }

        // Get => Management/RemoveUser
        [Authorize(Roles = "Developer")]
        public ActionResult RemoveUser(string email) {
            UserManagerResult removeUserResult = UserManager.RemoveUser(email);
            if (removeUserResult.Success) {
                TempData["UserRemoved"] = "User removed successfully";
                return RedirectToAction("Manage", "Management");
            }
            TempData["UserRemoved"] = removeUserResult.ErrorMessage;
            return RedirectToAction("Manage", "Management");
        }

        // Get => Management/EditUser
        [Authorize(Roles = "Developer")]
        public ActionResult EditUser(string email) {
            if (Roles.IsUserInRole(User.Identity.Name, "Developer")) {
                ManagementEditUserViewModel model = new ManagementEditUserViewModel();
                UserManagerResult getUserResult = UserManager.GetUser(email);
                if (!getUserResult.Success) {
                    ModelState.AddModelError("", "Some Thing went wrong");
                    return View(model);
                }
                model.Email = getUserResult.User.Email.Replace(" ", String.Empty);
                model.LastName = getUserResult.User.LastName.Replace(" ", String.Empty);
                model.FirstName = getUserResult.User.FirstName.Replace(" ", String.Empty);
                model.BirthDate = getUserResult.User.BirthDate.Replace(" ", String.Empty);
                model.Gender = getUserResult.User.Gender.Replace(" ", String.Empty);
                model.PhoneNumber = getUserResult.User.PhoneNumber;
                return View(model);
            }
            return RedirectToAction("Index", "Home");

            //return View(model);
        }


        // Post => Management/EditUser
        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(ManagementEditUserViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            if (Roles.IsUserInRole(User.Identity.Name, "Developer")) {
                Account userAccount = new Account {
                    Email = model.Email,
                    //we are gonna use RegistrationDate to deliver the new email
                    RegistrationDate = model.NewEmail,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                };
                UserManagerResult updateUserResult = UserManager.UpdateUser(User.Identity.Name, userAccount, model.ManagementPassword);
                if (updateUserResult.ErrorMessage != null) {
                    ModelState.AddModelError("", updateUserResult.ErrorMessage);
                    model.Password = "";
                    model.ConfirmPassword = "";
                    model.ManagementPassword = "";
                    return View(model);
                }
                if (updateUserResult.Success) {
                    if (User.Identity.Name == model.Email) {
                        return RedirectToAction("LogOut", "Account");
                    }
                    TempData["UserUpdated"] = "User updated successfully";
                    return RedirectToAction("Manage", "Management");
                }
                ModelState.AddModelError("", "Wrong Management password");
                return View(model);
            }
            return RedirectToAction("index", "Home");
        }

        // Get => Management/EditUser
        [Authorize(Roles = "Developer")]
        public ActionResult EditRoles(string email) {
            if (email == null) {
                return RedirectToAction("Manage", "Management");
            }
            if (Roles.IsUserInRole(User.Identity.Name, "Developer")) {
                UserRoleManagerResult getUserRoles = UserRoleManager.GetUserRoles(email);
                //if (getUserRoles.Success) {
                string roles = getUserRoles.Roles.Aggregate <string, string>(null, (current, role) => current + role + "             ");
                ManagementEditRolesViewModel model = new ManagementEditRolesViewModel {Email = email, Roles = roles,};
                UserRoleManagerResult getRoleListResult = UserRoleManager.GetRoles();

                if (getRoleListResult.Success) {
                    model.RoleList = getRoleListResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
                }
                else {
                    TempData["ManagementError"] = getRoleListResult.ErrorMessage;
                    ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
                }

                return View(model);
                //}
                //TempData["EditRole"] = "Something went wrong";
                //return RedirectToAction("Manage", "Management");
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRoles(ManagementEditRolesViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }

            UserRoleManagerResult getRoleListResult = UserRoleManager.GetRoles();
            if (getRoleListResult.Success) {
                model.RoleList = getRoleListResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            }
            else {
                TempData["ManagementError"] = getRoleListResult.ErrorMessage;
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
                return RedirectToAction("Manage", "Management");
            }

            UserRoleManagerResult getUserRoles = UserRoleManager.GetUserRoles(model.Email);
            if (getUserRoles.Success) {
                string roles = getUserRoles.Roles.Aggregate <string, string>(null, (current, role) => current + role + " ");
                model.Roles = roles;
            }
            else {
                TempData["ManagementError"] = getUserRoles.ErrorMessage;
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
                return RedirectToAction("Manage", "Management");
            }

            UserRoleManagerResult addRoleToUserResult = UserRoleManager.AddRoleToUser(model.Email, model.SelectedRole);
            if (addRoleToUserResult.Success) {
                ModelState.AddModelError("", "Role Added successfully");
                return View(model);
            }
            ModelState.AddModelError("", addRoleToUserResult.ErrorMessage);
            return View(model);
        }
    }
}