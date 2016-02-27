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
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    string[] allowedExtensions = new[] {".Jpg", ".png", ".jpg", ".jpeg", ".gif"};
                    string fileExt = Path.GetExtension(imageFile.FileName); // Get file Extension
                    // Check if file in the right extension
                    if (!allowedExtensions.Contains(fileExt)) {
                        vehicleManagerResult.ErrorMessage = "Wrong file Format";
                        return vehicleManagerResult;
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
                        vehicleManagerResult.ErrorMessage = "This license plate already exists";
                        return vehicleManagerResult;
                    }

                    Image newImage = Image.FromStream(imageFile.InputStream, true, true);
                    Image resizedImage = Generic.ResizeImage(newImage, 400, 250);
                    resizedImage.Save(path);
                    alenMotorsDbEntities.Vehicles.Add(newVehicle);
                    alenMotorsDbEntities.SaveChanges();
                    vehicleManagerResult.Success = true;
                    return vehicleManagerResult;
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
        }

        /// <summary>
        /// Gets all list of all Vehicles
        /// </summary>
        /// <returns>Returlss true along with a List<Vehicle> else will retunr a sting with the error message </returns>
        public static VehicleManagerResult GetVehicles() {
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        vehicleManagerResult.VehicleList.Add(vehicle);
                    }
                    vehicleManagerResult.Success = true;
                    return vehicleManagerResult;
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
        }


        /// <summary>
        /// Get the specifc Vehicle based on ID
        /// </summary>
        /// <param name="vehicleIdStr"></param>
        /// <returns>Returns true with the  object vehicle, else will retunr a sting with the error message</returns>
        public static VehicleManagerResult GetVehicle(string vehicleIdStr) {
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    int vehicleId = Int32.Parse(vehicleIdStr);
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList().Where(vehicle => vehicleId == vehicle.VehicleID)) {
                        vehicleManagerResult.Vehicle = vehicle;
                        vehicleManagerResult.Success = true;
                        return vehicleManagerResult;
                    }
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
            return null;
        }

        /// <summary>
        /// Edits a vehicle based on the passed information
        /// </summary>
        /// <param name="vehicleToEdit"></param>
        /// <param name="imageFile"></param>
        /// <param name="serverPath"></param>
        /// <returns>Returns true if the operation was successful else will return a stirng with the error message</returns>
        public static VehicleManagerResult EditVehicle(Vehicle vehicleToEdit, HttpPostedFileBase imageFile, string serverPath) {
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                string path = null;
                string oldPath = null;
                // with Image update
                if (imageFile != null) {
                    string[] allowedExtensions = new[] {".Jpg", ".png", ".jpg", ".jpeg", ".gif"};
                    string fileExt = Path.GetExtension(imageFile.FileName); // Get file Extension
                    // Check if file in the right extension
                    if (!allowedExtensions.Contains(fileExt)) {
                        vehicleManagerResult.ErrorMessage = "Wrong file Format";
                        return vehicleManagerResult;
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
                            vehicleManagerResult.Success = true;
                            return vehicleManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
            return null;
        }


        /// <summary>
        /// Removes a Vehicle based on ID
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="serverPath"></param>
        /// <returns>Returns true if the operation was successful else will retrun a string with the error message</returns>
        public static VehicleManagerResult RemoveVehicle(string vehicleID, string serverPath) {
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID == int.Parse(vehicleID)) {
                            string path = Path.Combine(serverPath, vehicle.ImageUrl);
                            File.Delete(path);
                            alenMotorsDbEntities.Vehicles.Remove(vehicle);
                            alenMotorsDbEntities.SaveChanges();
                            vehicleManagerResult.Success = true;
                            return vehicleManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
            return null;
        }

        public static VehicleManagerResult GetVehicleById(int id) {
            VehicleManagerResult vehicleManagerResult = new VehicleManagerResult();
            try {
                using (AlenMotorsDbEntities alenMotorsDbEntities = new AlenMotorsDbEntities()) {
                    foreach (Vehicle vehicle in alenMotorsDbEntities.Vehicles.ToList()) {
                        if (vehicle.VehicleID == id) {
                            vehicleManagerResult.Vehicle = vehicle;
                            vehicleManagerResult.Success = true;
                            return vehicleManagerResult;
                        }
                    }
                }
            }
            catch (Exception ex) {
                vehicleManagerResult.ErrorMessage = ex.Message;
                return vehicleManagerResult;
            }
            return null;
        }
    }
}