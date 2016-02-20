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

        public int DatyCount{get; set;}

        public List <Vehicle> Vehicles{get; set;}


        public OrderViewModel() {
            Vehicles = new List <Vehicle>();
        }
    }
}