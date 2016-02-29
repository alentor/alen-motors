using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlenMotorsDAL;
using Newtonsoft.Json;

namespace alenMotorsWeb.Models {
    public class ManagementAccountDisplayModelView {
        [Display(Name = "Email")]
        public string Email{get; set;}

        [Display(Name = "Last Name")]
        public string LastName{get; set;}

        [Display(Name = "First Name")]
        public string FirstName{get; set;}

        [Display(Name = "Birth Date")]
        public string BirthDate{get; set;}

        [Display(Name = "Gender")]
        public string Gender{get; set;}

        [Display(Name = "Phone Number")]
        public int PhoneNumber{get; set;}

        [Display(Name = "Roles")]
        public string Roles{get; set;}
    }

    public class ManagementEditRolesViewModel {
        [Display(Name = "Email")]
        public string Email{get; set;}

        [Display(Name = "Roles")]
        public string Roles{get; set;}

        public List <SelectListItem> RoleList{get; set;}

        public List <SelectListItem> RoleListUser{get; set;}

        [Display(Name = "Roles to Add")]
        public string RoleToAdd{get; set;}

        [Display(Name = "Roles to Remove")]
        public string RoleToRemove{get; set;}
    }

    public class ManagementViewModel {
        [Display(Name = "New Role")]
        public string NewRole{get; set;}

        [Display(Name = "New Branch")]
        public string NewBranch{get; set;}

        [Display(Name = "Remove Role")]
        public string RoleToRemove{get; set;}

        [Display(Name = "Remove Branch")]
        public string BranchToRemove{get; set;}


        public List <SelectListItem> RoleList{get; set;}

        public List <SelectListItem> BracnhList{get; set;}


        [Display(Name = "User list")]
        public List <Account> UserList{get; set;}

        public List <ManagementAccountDisplayModelView> AccountModels{get; private set;}

        public ManagementViewModel() {
            AccountModels = new List <ManagementAccountDisplayModelView>();
        }
    }

    public class ManagementEditUserViewModel {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Management Password")]
        public string ManagementPassword{get; set;}

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email{get; set;}

        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail{get; set;}

        [EmailAddress]
        [Display(Name = "Confirm Email")]
        [System.ComponentModel.DataAnnotations.Compare("NewEmail", ErrorMessage = "The Email and Email password do not match.")]
        public string ConfirmNewEmail{get; set;}

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password{get; set;}

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword{get; set;}

        [Display(Name = "First Name")]
        public string FirstName{get; set;}

        [Display(Name = "Last Name")]
        public string LastName{get; set;}

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public string BirthDate{get; set;}

        [Display(Name = "Gender")]
        public string Gender{get; set;}

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public int PhoneNumber{get; set;}
    }

    public class ViewAllOrdersViewModel {
        [Display(Name = "Search")]
        public string SearchStr{get; set;}
        public List <Order> Orders{get; set;}
        public List <Account> Accounts{get; set;}
        public List<Vehicle> Vehicles { get; set; }

    }

    public class EditOrderViewModel {
        [Display(Name = "Order ID")]
        public string OrderID{get; set;}
        [Required]
        [Display(Name = "Email")]
        public string Email{get; set;}

        [Required]
        [Display(Name = "Start Date")]
        public string StartDate{get; set;}

        [Required]
        [Display(Name = "End Date")]
        public string EndDate{get; set;}


        [Required]
        [Display(Name = "Order Status")]
        public string StatusStr{get; set;}
    }

    public class ManagementViewVehicle {
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

    }
}