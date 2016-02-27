using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using alenMotorsWeb.Models;
using AlenMotorsDAL;
using Microsoft.ApplicationInsights.Channel;

namespace alenMotorsWeb.Controllers {
    public class GarageController: Controller {
        [Authorize(Roles = "Employee")]
        // GET => Garage/Index
        public ActionResult Index() {
            GarageViewModel model = new GarageViewModel();
            if (TempData["AddVehicle"] != null) {
                ModelState.AddModelError(string.Empty, TempData["AddVehicle"].ToString());
            }
            if (TempData["EditVehicle"] != null) {
                ModelState.AddModelError(string.Empty, TempData["EditVehicle"].ToString());
            }
            if (TempData["RemoveVehicle"] != null) {
                ModelState.AddModelError(string.Empty, TempData["RemoveVehicle"].ToString());
            }
            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicles();
            if (!vehicleManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                return View(model);
            }
            BranchManagerResult branchManagerResult = BranchManager.GetBrances();
            if (!branchManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                return View(model);
            }
            model.BrancheList = branchManagerResult.Branches;
            model.VehicleList = vehicleManagerResult.VehicleList;
            return View(model);
        }

        // GET => Garage/AddVehicle
        [Authorize(Roles = "Manager")]
        public ActionResult AddVehicle() {
            GarageAddNewVehicleViewModel model = new GarageAddNewVehicleViewModel();
            // branches for DropDown
            BranchManagerResult branchManagerResult = BranchManager.GetBranchesNames();
            if (branchManagerResult.Success) {
                model.BracnhList = branchManagerResult.BranchesNames.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
            }
            else {
                TempData["AddVehicle"] = branchManagerResult.ErrorMessage;
                return RedirectToAction("Index", "Garage");
            }
            return View(model);
        }

        // POST => Garage/AddVehicle
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVehicle(GarageAddNewVehicleViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return View(model);
            }
            Vehicle newVehicle = new Vehicle {
                LicensePlate = model.LicensePlate,
                Transmission = model.Transmission,
                Branch = new Branch {BranchName = model.BranchSelected},
                Color = model.Color,
                DistanceTraveled = model.DistanceTraveled,
                Model = model.VehicleModel,
                ManufacturYear = model.ManufacturYear,
                Manufacturer = model.Manufacturer,
                Price = model.Price,
                Status = "Available"
            };

            string serverPath = Server.MapPath("~/Content/GarageImages");
            VehicleManagerResult vehicleManagerResult = VehicleManager.AddNewVehicle(newVehicle, model.ImageFile, serverPath);
            if (!vehicleManagerResult.Success) {
                ModelState.AddModelError("", vehicleManagerResult.ErrorMessage);
                BranchManagerResult getBranchesResult = BranchManager.GetBranchesNames();
                if (getBranchesResult.Success) {
                    model.BracnhList = getBranchesResult.BranchesNames.Select(x => new SelectListItem {Text = x, Value = x}).ToList();
                    return View(model);
                }
                TempData["AddVehicle"] = getBranchesResult.ErrorMessage;
                return RedirectToAction("Index", "Garage");
            }
            TempData["AddVehicle"] = "Vehicle Added";
            return RedirectToAction("Index", "Garage");
        }

        // GET => Garage/EditVehicle
        [Authorize(Roles = "Manager")]
        public ActionResult EditVehicle(string vehicleID) {
            VehicleManagerResult vehicleManagerResult = VehicleManager.GetVehicle(vehicleID);
            if (!vehicleManagerResult.Success) {
                TempData["EditVehicle"] = vehicleManagerResult.ErrorMessage;
                return RedirectToAction("Index", "Garage");
            }
            GarageEditVehicleViewModel model = new GarageEditVehicleViewModel {
                DistanceTraveled = vehicleManagerResult.Vehicle.DistanceTraveled,
                ManufacturYear = vehicleManagerResult.Vehicle.ManufacturYear,
                Manufacturer = vehicleManagerResult.Vehicle.Manufacturer.Replace(" ", String.Empty),
                Color = vehicleManagerResult.Vehicle.Color.Replace(" ", String.Empty),
                LicensePlate = vehicleManagerResult.Vehicle.LicensePlate,
                VehicleModel = vehicleManagerResult.Vehicle.Model.Replace(" ", String.Empty),
                ImageFileUrl = vehicleManagerResult.Vehicle.ImageUrl.Replace(" ", String.Empty),
                Transmission = vehicleManagerResult.Vehicle.Transmission.Replace(" ", String.Empty),
                VehicleId = vehicleManagerResult.Vehicle.VehicleID,
                Status = vehicleManagerResult.Vehicle.Status,
                Price = vehicleManagerResult.Vehicle.Price
            };
            // Bbranch list
            BranchManagerResult getBranchesResult = BranchManager.GetBranchesNames();
            if (!getBranchesResult.Success) {
                TempData["EditVehicle"] = getBranchesResult.ErrorMessage;
                return RedirectToAction("Index", "Garage");
            }
            BranchManagerResult branchManagerResult = BranchManager.GetBranch(vehicleManagerResult.Vehicle.BranchID);
            if (!branchManagerResult.Success) {
                TempData["EditVehicle"] = getBranchesResult.ErrorMessage;
                return RedirectToAction("Index", "Garage");
            }

            model.BrancSelected = branchManagerResult.Branch.BranchName;
            getBranchesResult.BranchesNames.Remove(branchManagerResult.Branch.BranchName.Replace(" ", String.Empty));
            model.BracnhList = getBranchesResult.BranchesNames.Select(x => new SelectListItem {Text = x, Value = x}).ToList();

            return View(model);
        }

        // POST=> Garage/EditVehicle
        [HttpPost]
        public ActionResult EditVehicle(GarageEditVehicleViewModel model) {
            if (!ModelState.IsValid) {
                ModelState.AddModelError("", "Model validation Error");
                return RedirectToAction("Index", "Garage");
            }
            Vehicle vehicleToEdit = new Vehicle {
                VehicleID = model.VehicleId,
                Branch = new Branch {BranchName = model.BrancSelected},
                Color = model.Color,
                DistanceTraveled = model.DistanceTraveled,
                ManufacturYear = model.ManufacturYear,
                Manufacturer = model.Manufacturer,
                Model = model.VehicleModel,
                LicensePlate = model.LicensePlate,
                Transmission = model.Transmission,
                Status = model.Status,
                Price = model.Price
            };
            string serverPath = Server.MapPath("~/Content/GarageImages");
            VehicleManagerResult vehicleManagerResult = VehicleManager.EditVehicle(vehicleToEdit, model.ImageFile, serverPath);
            if (vehicleManagerResult.Success) {
                TempData["EditVehicle"] = "Vehicle updated uccessfully ";
                return RedirectToAction("Index", "Garage");
            }
            TempData["EditVehicle"] = vehicleManagerResult.ErrorMessage;
            return RedirectToAction("Index", "Garage", new {vehicleID = model.VehicleId.ToString()});
        }

        public ActionResult RemoveVehicle(string vehicleID) {
            string serverPath = Server.MapPath("~/Content/GarageImages");
            VehicleManagerResult vehicleManagerResult = VehicleManager.RemoveVehicle(vehicleID, serverPath);
            if (vehicleManagerResult.Success) {
                TempData["RemoveVehicle"] = "Vehicle removed successfully ";
                return RedirectToAction("Index", "Garage");
            }
            TempData["EditVehicle"] = vehicleManagerResult.ErrorMessage;
            return RedirectToAction("Index", "Garage");
        }
    }
}