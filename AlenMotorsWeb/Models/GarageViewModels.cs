using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using AlenMotorsDAL;

namespace alenMotorsWeb.Models {
    public class GarageAddNewVehicleViewModel {
        [Required]
        [Display(Name = "License Plate")]
        [Range(10000000, 99999999)]
        public int LicensePlate{get; set;}

        [Required]
        [Display(Name = "Manufacturer")]
        public string Manufacturer{get; set;}

        [Required]
        [Display(Name = "Manufactur Year")]
        [Range(1920, 2016)]
        public int ManufacturYear{get; set;}

        [Required]
        [Display(Name = "Model")]
        public string VehicleModel{get; set;}

        [Required]
        [Display(Name = "Mileage")]
        public int DistanceTraveled{get; set;}

        [Required]
        [Display(Name = "Transmission")]
        public string Transmission{get; set;}
        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string Color{get; set;}

        [Required]
        [Display(Name = "Branch")]
        public string BranchSelected{get; set;}

        public List <SelectListItem> BracnhList{get; set;}
        
        

        [Display(Name = "ImageFile")]
        [Required(ErrorMessage = "You must provid an Image")]
        public HttpPostedFileBase ImageFile{get; set;}
    }


    public class GarageEditVehicleViewModel {

        [Required]
        [Display(Name = "License Plate")]
        [Range(10000000, 99999999)]
        public int LicensePlate{get; set;}

        [Required]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Manufactur Year")]
        [Range(1920, 2016)]
        public int ManufacturYear { get; set; }

        [Required]
        [Display(Name = "Model")]
        public string VehicleModel { get; set; }

        [Required]
        [Display(Name = "Mileage")]
        public int DistanceTraveled{get; set;}


        [Display(Name = "Transmission")]
        public string Transmission { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string Color{get; set;}

        [Display(Name = "Branch")]
        public string BrancSelected { get; set; }

        [Display(Name = "Price")]
        public int Price{get; set;}

        public int VehicleId{get; set;}

        public List <SelectListItem> BracnhList{get; set;}

        [Display(Name = "ImageFile")]
        public HttpPostedFileBase ImageFile{get; set;}

        [Display(Name = "ImageFile")]
        public string ImageFileUrl{get; set;}
    }

    public class GarageViewModel {
        public List <Vehicle> VehicleList{get; set;}
        public List <Branch> BrancheList{get; set;}
    }
}