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
        // GET => Management
        [Authorize(Roles = "Employee")]
        public ActionResult Index(ManagementViewModel model, int? page) {
            // Error Filtering
            if (TempData["RoleAdded"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RoleAdded"].ToString());
            }
            if (TempData["BranchAdded"] != null) {
                ModelState.AddModelError(string.Empty, TempData["BranchAdded"].ToString());
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
            if (TempData["ViewAllOrders"] != null) {
                ModelState.AddModelError(string.Empty, TempData["ViewAllOrders"].ToString());
            }

            // Roels for DropDown
            UserRoleManagerResult getRolesResult = UserRoleManager.GetRoles();
            if (getRolesResult.Success) {
                model.RoleList = getRolesResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            }
            else {
                TempData["ManagementError"] = getRolesResult.ErrorMessage;
                ModelState.AddModelError(string.Empty, TempData["ManagementError"].ToString());
            }

            // branches for DropDown
            BranchManagerResult getBranchesResult = BranchManager.GetBranchesNames();
            if (getBranchesResult.Success) {
                model.BracnhList = getBranchesResult.BranchesNames.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            }
            else {
                TempData["ManagementError"] = getBranchesResult.ErrorMessage;
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
            }

            return View(model);
        }

        // Post => Management
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult AddNewRole(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Management");
            }
            UserRoleManagerResult addNewRole = UserRoleManager.AddRole(model.NewRole);
            if (addNewRole.Success) {
                TempData["RoleAdded"] = "Roled Added";
            }
            else {
                TempData["RoleAdded"] = addNewRole.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            return RedirectToAction("Index", "Management");
        }


        // Post => Management/RemoveRole
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult RemvoeRole(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Management");
            }
            UserRoleManagerResult removeRole = UserRoleManager.RemoveRole(model.RoleToRemove);
            if (removeRole.Success) {
                TempData["RoleRemoved"] = "Role Removed";
            }
            else {
                TempData["RoleRemoved"] = removeRole.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            return RedirectToAction("Index", "Management");
        }

        // Post => Management/AddBranch
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult AdddNewBranch(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Management");
            }
            BranchManagerResult addNewBranch = BranchManager.AddBranch(model.NewBranch);
            if (addNewBranch.Success) {
                TempData["BranchAdded"] = "Branch Added";
            }
            else {
                TempData["RoleAdded"] = addNewBranch.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            return RedirectToAction("Index", "Management");
        }

        // Post => Management/RemoveBranch
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult RemoveBranch(ManagementViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Management");
            }
            BranchManagerResult removeBranch = BranchManager.RemoveBranch(model.BranchToRemove);
            if (removeBranch.Success) {
                TempData["BranchAdded"] = "Branch Removed";
            }
            else {
                TempData["RoleAdded"] = removeBranch.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            return RedirectToAction("Index", "Management");
        }


        // Get => Management/RemoveUser
        [Authorize(Roles = "Developer")]
        public ActionResult RemoveUser(string email) {
            UserManagerResult removeUserResult = UserManager.RemoveUser(email);
            if (removeUserResult.Success) {
                TempData["UserRemoved"] = "User removed successfully";
                return RedirectToAction("Index", "Management");
            }
            TempData["UserRemoved"] = removeUserResult.ErrorMessage;
            return RedirectToAction("Index", "Management");
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

        // Get => Management/EditUser
        [Authorize(Roles = "Developer")]
        public ActionResult EditRoles(string email) {
            if (email == null) {
                return RedirectToAction("Index", "Management");
            }
            if (TempData["AddRoleToUser"] != null) {
                ModelState.AddModelError(string.Empty, TempData["AddRoleToUser"].ToString());
            }
            if (TempData["RemoveRoleUser"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RemoveRoleUser"].ToString());
            }
            UserRoleManagerResult getUserRolesResult = UserRoleManager.GetUserRoles(email);
            if (!getUserRolesResult.Success) {
                TempData["EditRole"] = getUserRolesResult.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            string roles = getUserRolesResult.Roles.Aggregate <string, string>(null, (current, role) => current + role + "             ");
            UserRoleManagerResult getRoleListResult = UserRoleManager.GetRoles();
            if (!getRoleListResult.Success) {
                TempData["EditRole"] = getUserRolesResult.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            ManagementEditRolesViewModel model = new ManagementEditRolesViewModel {
                Email = email,
                Roles = roles,
                RoleList = getRoleListResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList(),
                RoleListUser = getUserRolesResult.Roles.Select(x => new SelectListItem {Text = x, Value = x}).ToList(),
            };
            return View(model);
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
                UserManagerResult updateUserResult = UserManager.EditUser(User.Identity.Name, userAccount, model.ManagementPassword);
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
                    return RedirectToAction("Index", "Management");
                }
                ModelState.AddModelError("", "Wrong Management password");
                return View(model);
            }
            return RedirectToAction("index", "Home");
        }


        // Post => Management/AddRoleToUSER
        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToUser(ManagementEditRolesViewModel model) {
            if (!ModelState.IsValid) {
                TempData["AddRoleToUser"] = "Model validation Error";
                return RedirectToAction("Index", "Management");
            }

            UserRoleManagerResult addRoleToUserResult = UserRoleManager.AddRoleToUser(model.Email, model.RoleToAdd);
            if (addRoleToUserResult.Success) {
                TempData["AddRoleToUser"] = "Role Added successfully";
                return RedirectToAction("EditRoles", "Management", new {email = model.Email});
            }
            TempData["AddRoleToUser"] = addRoleToUserResult.ErrorMessage;
            return RedirectToAction("EditRoles", "Management", new {email = model.Email});
        }

        // Post => Management/RemoveRoleFromUser
        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveRoleFromUser(ManagementEditRolesViewModel model) {
            if (!ModelState.IsValid) {
                TempData["AddRoleToUser"] = "Model validation Error";
                return RedirectToAction("Index", "Management");
            }

            UserRoleManagerResult removeRoleFromUserResult = UserRoleManager.RemoveRoleFromUser(model.Email, model.RoleToRemove);
            if (removeRoleFromUserResult.Success) {
                TempData["AddRoleToUser"] = "Role Removed successfully";
                return RedirectToAction("EditRoles", "Management", new {email = model.Email});
            }
            TempData["AddRoleToUser"] = removeRoleFromUserResult.ErrorMessage;
            return RedirectToAction("EditRoles", "Management", new {email = model.Email});
        }

        // Get => Management/ViewAllOrders
        [Authorize(Roles = "Employee")]
        public ActionResult ViewAllOrders(string searchStr, string searchType) {
            ViewAllOrdersViewModel model;
            switch (searchType) {
                case "contains":{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    SearchManagerResult searchManagerResult = SearchManager.ViewOrdersContainsSearch(searchStr);
                    if (!searchManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = searchManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    ModelState.AddModelError("", model.Orders.Count + " Results");
                }
                    break;

                case "orderId":{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    SearchManagerResult searchManagerResult = SearchManager.ViewOrdersOrderIdSearch(searchStr);
                    if (!searchManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = searchManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    ModelState.AddModelError("", model.Orders.Count + " Results");
                }
                    break;

                case "orderIdGreaterThan":{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    SearchManagerResult searchManagerResult = SearchManager.ViewOrdersOrderIdGreaterThanSearch(searchStr);
                    if (!searchManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = searchManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    ModelState.AddModelError("", model.Orders.Count + " Results");
                }
                    break;

                case "orderIdLessThan":{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    SearchManagerResult searchManagerResult = SearchManager.ViewOrdersOrderIdLessThanSearch(searchStr);
                    if (!searchManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = searchManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    ModelState.AddModelError("", model.Orders.Count + " Results");
                }
                    break;

                case "Email":{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    SearchManagerResult searchManagerResult = SearchManager.ViewOrdersEmaildSearch(searchStr);
                    if (!searchManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = searchManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    ModelState.AddModelError("", model.Orders.Count + " Results");
                }
                    break;

                default:{
                    VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
                    if (!vehicleManagerResult.Success) {
                        ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                        return RedirectToAction("Account", "Account");
                    }
                    OrderManagerResult orderManagerResult = OrderManager.GetAllOrders();
                    if (!orderManagerResult.Success) {
                        TempData["ViewAllOrders"] = orderManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    UserManagerResult userManagerResult = UserManager.GetUsers();
                    if (!userManagerResult.Success) {
                        TempData["ViewAllOrders"] = userManagerResult.ErrorMessage;
                        return RedirectToAction("Index", "Management");
                    }
                    if (TempData["EditOrder"] != null) {
                        ModelState.AddModelError(string.Empty, TempData["EditOrder"].ToString());
                    }
                    model = new ViewAllOrdersViewModel {
                        Orders = orderManagerResult.Orders,
                        Accounts = userManagerResult.UserList,
                        Vehicles = vehicleManagerResult.VehicleList
                    };
                    break;
                }
            }
            return View(model);
        }

        // Post => Management/ViewOrdersContainsSearch
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult ViewOrdersContainsSearch(string searchStr) {
            return RedirectToAction("ViewAllOrders", "Management", new {searchStr, searchType = "contains"});
        }

        // Post => Management/ViewOrdersOrderIDSearch
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult ViewOrdersOrderIdSearch(string searchStr) {
            return RedirectToAction("ViewAllOrders", "Management", new {searchStr, searchType = "orderId"});
        }

        // Post => Management/ViewOrdersOrderIDSearch
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult ViewOrdersOrderIdGreaterThanSearch(string searchStr) {
            return RedirectToAction("ViewAllOrders", "Management", new {searchStr, searchType = "orderIdGreaterThan" });
        }

        // Post => Management/ViewOrdersOrderIDSearch
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult ViewOrdersOrderIdLessThanSearch(string searchStr) {
            return RedirectToAction("ViewAllOrders", "Management", new {searchStr, searchType = "orderIdLessThan"});
        }

        // Post => Management/ViewOrdersOrderIDSearch
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public ActionResult ViewOrdersEmailSearch(string searchStr)
        {
            return RedirectToAction("ViewAllOrders", "Management", new { searchStr, searchType = "Email" });
        }

        // Get => Management/EditOrders
        [Authorize(Roles = "Employee")]
        public ActionResult EditOrder(string orderID) {
            //Order orderToEdit = new Order {OrderID = int.Parse(orderID)};
            OrderManagerResult orderManagerResult = OrderManager.GetOrder(orderID);
            if (!orderManagerResult.Success) {
                TempData["EditOrder"] = orderManagerResult.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            UserManagerResult userManagerResult = UserManager.GetUserByUserID(orderManagerResult.Order.AccountID);
            if (!userManagerResult.Success) {
                TempData["EditOrder"] = orderManagerResult.ErrorMessage;
                return RedirectToAction("Index", "Management");
            }
            EditOrderViewModel model = new EditOrderViewModel {
                StartDate = orderManagerResult.Order.StartDate.Replace(" ", String.Empty),
                EndDate = orderManagerResult.Order.EndDate.Replace(" ", String.Empty),
                Email = userManagerResult.User.Email,
                OrderID = orderManagerResult.Order.OrderID.ToString()
            };
            model.StatusStr = orderManagerResult.Order.Status ? "Closed" : "Opened";
            return View(model);
        }

        // Post => Management/EditOrders
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public ActionResult EditOrder(EditOrderViewModel model) {
            if (!ModelState.IsValid) {
                TempData["EditOrder"] = "Model validation Error";
                return RedirectToAction("ViewAllOrders", "Management");
            }
            OrderManagerResult orderManagerResult;
            if (model.StatusStr == "Opened") {
                orderManagerResult =
                OrderManager.EditOrder(
                                       new Order {
                                           OrderID = int.Parse(model.OrderID),
                                           StartDate = model.StartDate,
                                           EndDate = model.EndDate,
                                           Status = false
                                       },
                                       model.Email);
                if (!orderManagerResult.Success) {
                    TempData["EditOrder"] = orderManagerResult.ErrorMessage;
                    return RedirectToAction("ViewAllOrders", "Management");
                }
                TempData["EditOrder"] = "Order edited successfully";
                return RedirectToAction("ViewAllOrders", "Management");
            }
            orderManagerResult =
            OrderManager.EditOrder(
                                   new Order {OrderID = int.Parse(model.OrderID), StartDate = model.StartDate, EndDate = model.EndDate, Status = true},
                                   model.Email);
            if (!orderManagerResult.Success) {
                TempData["EditOrder"] = orderManagerResult.ErrorMessage;
                return RedirectToAction("ViewAllOrders", "Management");
            }
            TempData["EditOrder"] = "Order edited successfully";
            return RedirectToAction("ViewAllOrders", "Management");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult RemoveOrder(string OrderID) {
            OrderManagerResult orderManagerResult = OrderManager.RemoveOrder(Int32.Parse(OrderID));
            if (!orderManagerResult.Success) {
                TempData["EditOrder"] = orderManagerResult.ErrorMessage;
                return RedirectToAction("ViewAllOrders", "Management");
            }
            TempData["EditOrder"] = "Order Removed successfully";
            return RedirectToAction("ViewAllOrders", "Management");
        }


        public ActionResult ViewVehicle(string vehicleID) {
            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicleById(int.Parse(vehicleID));
            if (!vehicleManagerResult.Success) {
                TempData["EditOrder"] = vehicleManagerResult.ErrorMessage;
                return RedirectToAction("ViewAllOrders", "Management");
            }
            ManagementViewVehicle model = new ManagementViewVehicle {
                Color = vehicleManagerResult.Vehicle.Color,
                DistanceTraveled = vehicleManagerResult.Vehicle.DistanceTraveled,
                Manufacturer = vehicleManagerResult.Vehicle.Manufacturer,
                ManufacturYear = vehicleManagerResult.Vehicle.ManufacturYear,
                VehicleModel = vehicleManagerResult.Vehicle.Model,
                LicensePlate = vehicleManagerResult.Vehicle.LicensePlate,
                Transmission = vehicleManagerResult.Vehicle.Transmission,
                Price = vehicleManagerResult.Vehicle.Price
            };

            return View(model);
        }
    }
}