using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlenMotorsDAL;

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

        [Display(Name = "User list")]
        public string SelectedRole{get; set;}
    }

    public class ManagementViewModel {
        [Display(Name = "New Role")]
        public string NewRole{get; set;}
        [Display(Name = "Remove Role")]
        public string RoleToRemove{get; set;}

        public List <SelectListItem> RoleList{get; set;}

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
}