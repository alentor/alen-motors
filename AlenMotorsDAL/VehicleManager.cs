using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AlenMotorsDAL {
    public class VehicleManager {
        /// <summary>
        /// Add a new Vehicle
        /// </summary>
        /// <param name="newVehicle"> New Vehicle class</param>
        /// <param name="imageFile"> Vehicle Image</param>
        /// <param name="serverPath"> Serer path used used to save the image</param>
        /// <returns>Retuns true if the addition has Succeeded, else returns string with an error message</returns>
        public static VehicleManagerResult AddNewVehicle(Vehicle newVehicle, HttpPostedFileBase imageFile, string serverPath) {
            VehicleManagerResult garageManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    string[] allowedExtensions = new[] {".Jpg", ".png", ".jpg", ".jpeg", ".gif"};
                    string fileExt = Path.GetExtension(imageFile.FileName); // Get file Extension
                    // Check if file in the right extension
                    if (!allowedExtensions.Contains(fileExt)) {
                        garageManagerResult.ErrorMessage = "Wrong file Format";
                        return garageManagerResult;
                    }
                    foreach (Branch branch in alenMotorsDbEntities.Branches.ToList()) {
                        if (branch.BranchName.Replace(" ", String.Empty) == newVehicle.Branch.BranchName) {
                            newVehicle.Branch = branch;
                        }
                    }

                    string fileName = Path.GetFileName(imageFile.FileName); // Get only file name
                    string path = System.IO.Path.Combine(serverPath, fileName); // Path to where the image will be saved to
                    newVehicle.ImageUrl = fileName;
                    if (alenMotorsDbEntities.Vehicles.ToList().Any(vehicle => vehicle.LicensePlate == newVehicle.LicensePlate)) {
                        garageManagerResult.ErrorMessage = "This license plate already exists";
                        return garageManagerResult;
                    }

                    Image newImage = Image.FromStream(imageFile.InputStream, true, true);
                    Image resizedImage = Generic.ResizeImage(newImage, 400, 250);
                    resizedImage.Save(path);
                    alenMotorsDbEntities.Vehicles.Add(newVehicle);
                    alenMotorsDbEntities.SaveChanges();
                    garageManagerResult.Success = true;
                    return garageManagerResult;
                }
            }
            catch (Exception ex) {
                garageManagerResult.ErrorMessage = ex.Message;
                return garageManagerResult;
            }
        }

        public static VehicleManagerResult GetVehicles() {
            VehicleManagerResult garageManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        garageManagerResult.VehicleList.Add(vehicle);
                    }
                    garageManagerResult.Success = true;
                    return garageManagerResult;
                }
            }
            catch (Exception ex) {
                garageManagerResult.ErrorMessage = ex.Message;
                return garageManagerResult;
            }
        }

        public static VehicleManagerResult GetVehicle(string vehicleIdStr) {
            VehicleManagerResult garageManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    int vehicleId = Int32.Parse(vehicleIdStr);
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList().Where(vehicle => vehicleId == vehicle.VehicleID)) {
                        garageManagerResult.Vehicle = vehicle;
                        garageManagerResult.Success = true;
                        return garageManagerResult;
                    }
                }
            }
            catch (Exception ex) {
                garageManagerResult.ErrorMessage = ex.Message;
                return garageManagerResult;
            }
            return null;
        }

        public static VehicleManagerResult EditVehicle(Vehicle vehicleToEdit, HttpPostedFileBase imageFile, string serverPath) {
            VehicleManagerResult garageManagerResult = new VehicleManagerResult();
            try {
                string path = null;
                string oldPath = null;
                // with Image update
                if (imageFile != null) {
                    string[] allowedExtensions = new[] {".Jpg", ".png", ".jpg", ".jpeg", ".gif"};
                    string fileExt = Path.GetExtension(imageFile.FileName); // Get file Extension
                    // Check if file in the right extension
                    if (!allowedExtensions.Contains(fileExt)) {
                        garageManagerResult.ErrorMessage = "Wrong file Format";
                        return garageManagerResult;
                    }
                    path = Path.Combine(serverPath, imageFile.FileName); // Path to where the image will be saved to
                }
                // without Image update
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID == vehicleToEdit.VehicleID) {
                            vehicle.DistanceTraveled = vehicleToEdit.DistanceTraveled;
                            vehicle.Color = vehicleToEdit.Color;
                            vehicle.ManufacturYear = vehicleToEdit.ManufacturYear;
                            vehicle.Manufacturer = vehicleToEdit.Manufacturer;
                            vehicle.LicensePlate = vehicleToEdit.LicensePlate;
                            vehicle.Price = vehicleToEdit.Price;
                            if (vehicleToEdit.Transmission != null) {
                                vehicle.Transmission = vehicleToEdit.Transmission;
                            }
                            if (vehicleToEdit.Status != null) {
                                vehicle.Status = vehicleToEdit.Status;
                            }
                            if (vehicleToEdit.Branch.BranchName != null) {
                                foreach (Branch branch in alenMotorsDbEntities.Branches.ToList()) {
                                    if (branch.BranchName.Replace(" ", String.Empty) == vehicleToEdit.Branch.BranchName) {
                                        vehicle.Branch = branch;
                                        vehicle.BranchID = branch.BranchID;
                                    }
                                }
                            }
                            if (path != null) {
                                Image newImage = Image.FromStream(imageFile.InputStream, true, true);
                                Image resizedImage = Generic.ResizeImage(newImage, 400, 250);
                                resizedImage.Save(path);
                                //newImage.Save(path);
                                //Generic.FormatPicture(imageFile, path, 400,400);
                                //imageFile.SaveAs(path);
                                oldPath = Path.Combine(serverPath, vehicle.ImageUrl);
                                if (File.Exists(oldPath)) {
                                    File.Delete(oldPath);
                                    vehicle.ImageUrl = imageFile.FileName;
                                }
                            }
                            alenMotorsDbEntities.SaveChanges();
                            garageManagerResult.Success = true;
                            return garageManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                garageManagerResult.ErrorMessage = ex.Message;
                return garageManagerResult;
            }
            return null;
        }

        public static VehicleManagerResult RemoveVehicle(string vehicleID, string serverPath) {
            VehicleManagerResult garageManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID == int.Parse(vehicleID)) {
                            string path = Path.Combine(serverPath, vehicle.ImageUrl);
                            File.Delete(path);
                            alenMotorsDbEntities.Vehicles.Remove(vehicle);
                            alenMotorsDbEntities.SaveChanges();
                            garageManagerResult.Success = true;
                            return garageManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                garageManagerResult.ErrorMessage = ex.Message;
                return garageManagerResult;
            }
            return null;
        }
    }
}