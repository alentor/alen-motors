using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AlenMotorsDAL;

namespace alenMotorsWeb.Models {
    public class OrderViewModel {
        [Required]
        [Display(Name = "Pickup Date")]
        public string StartDate{get; set;}

        [Required]
        [Display(Name = "Return Date")]
        public string EndDate{get; set;}

        public int DayCount{get; set;}

        public List <Vehicle> Vehicles{get; set;}


        public OrderViewModel() {
            Vehicles = new List <Vehicle>();
        }
    }

    public class OrderListUserViewModel {
        public List <Order> OrderList{get; set;}

        public List <Vehicle> Vehicles{get; set;}
    }

    public class OrdertViewVehicle {
        [Display(Name = "License Plate")]
        public int LicensePlate{get; set;}

        [Display(Name = "Manufacturer")]
        public string Manufacturer{get; set;}

        [Display(Name = "Manufactur Year")]
        [Range(1920, 2016)]
        public int ManufacturYear{get; set;}

        [Display(Name = "Model")]
        public string VehicleModel{get; set;}

        [Display(Name = "Mileage")]
        public int DistanceTraveled{get; set;}

        [Display(Name = "Transmission")]
        public string Transmission{get; set;}

        [Display(Name = "Price")]
        public int Price{get; set;}

        [Display(Name = "Color")]
        public string Color{get; set;}

        [Display(Name = "Branch")]
        public string BranchSelected{get; set;}

        [Required(ErrorMessage = "You must provid an Image")]
        public HttpPostedFileBase ImageFile{get; set;}
    }
}