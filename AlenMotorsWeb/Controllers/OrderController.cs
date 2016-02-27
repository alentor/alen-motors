using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alenMotorsWeb.Models;
using AlenMotorsDAL;

namespace alenMotorsWeb.Controllers {
    public class OrderController: Controller {
        // GET => Order
        [AllowAnonymous]
        public ActionResult Index() {
            OrderViewModel model = new OrderViewModel();
            if (TempData["MakeAnOrder"] != null) {
                ModelState.AddModelError(string.Empty, TempData["MakeAnOrder"].ToString());
            }
            return View(model);
        }

        //[Authorize(Roles = "User")]
        [AllowAnonymous]
        // POST => Order
        [HttpPost]
        public ActionResult Index(OrderViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Order");
            }
            DateTime startDate;
            DateTime endDate;
            try {
                startDate = DateTime.ParseExact(model.StartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDate = DateTime.ParseExact(model.EndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            TimeSpan timeSpan = endDate - startDate;
            int dayCount = timeSpan.Days == 0 ? 1 : timeSpan.Days;
            OrderViewModel modelToReturn = new OrderViewModel {EndDate = model.EndDate, StartDate = model.StartDate, DayCount = dayCount};

            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
            if (!vehicleManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                return View();
            }
            foreach (Vehicle vehicle in vehicleManagerResult.VehicleList.Where(vehicle => vehicle.Status.Replace(" ", String.Empty) == "Available")) {
                modelToReturn.Vehicles.Add(vehicle);
            }
            return View(modelToReturn);
        }

        // GET => Order/MakeAnOrder
        [Authorize(Roles = "User")]
        public ActionResult MakeAnOrder(string vehicleID, string startDate, string endDate, string dayCount) {
            OrderManagerResult orderManagerResult = OrderManager.AddOrder(User.Identity.Name, vehicleID, startDate, endDate, dayCount);
            if (!orderManagerResult.Success) {
                TempData["MakeAnOrder"] = orderManagerResult.ErrorMessage;
                return RedirectToAction("Index", "Order");
            }
            ModelState.AddModelError("", "Ordered added Successfully");
            return RedirectToAction("Account", "Account");
        }

        // GET => Order/ViewOrders
        [Authorize(Roles = "User")]
        public ActionResult ViewOrders() {
            OrderManagerResult orderManagerResult = OrderManager.GetOrders(User.Identity.Name);
            if (!orderManagerResult.Success) {
                ModelState.AddModelError("", orderManagerResult.ErrorMessage);
                return RedirectToAction("Account", "Account");
            }
            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
            if (!vehicleManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                return RedirectToAction("Account", "Account");
            }
            OrderListUserViewModel model = new OrderListUserViewModel {OrderList = orderManagerResult.Orders, Vehicles = vehicleManagerResult.VehicleList};
            return View(model);
        }

        public ActionResult ViewVehicle(string vehicleID) {
            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicleById(int.Parse(vehicleID));
            if (!vehicleManagerResult.Success) {
                TempData["EditOrder"] = vehicleManagerResult.ErrorMessage;
                return RedirectToAction("ViewAllOrders", "Management");
            }
            OrdertViewVehicle model = new OrdertViewVehicle {
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