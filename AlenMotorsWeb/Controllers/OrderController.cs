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
        public ActionResult Index() {
            OrderViewModel model = new OrderViewModel();
            return View(model);
        }

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
            int daysToRent = timeSpan.Days;
            OrderViewModel modelToReturn = new OrderViewModel {EndDate = model.EndDate, StartDate = model.StartDate, DatyCount = daysToRent};

            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
            if (!vehicleManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                return View();
            }
            foreach (Vehicle vehicle in vehicleManagerResult.VehicleList.Where(vehicle => vehicle.Status.Replace(" ", String.Empty) == "Avaliable")) {
                modelToReturn.Vehicles.Add(vehicle);
            }
            return View(modelToReturn);
        }

        public ActionResult MakeAnOrder(string vehicleID, string startDate, string endDate) {
            return RedirectToAction("Index", "Home");
        }
    }
}