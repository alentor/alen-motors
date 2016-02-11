using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.SqlServer.Server;

namespace alenMotorsWeb.Models {
    public class LoginViewModel {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email{get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password{get; set;}

        [Display(Name = "Remember me?")]
        public bool RememberMe{get; set;}
    }

    public class RegisterViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email{get; set;}

        [EmailAddress]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "The Email and Email password do not match.")]
        public string ConfirmEmail{get; set;}

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password{get; set;}

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword{get; set;}

        [Required(ErrorMessage = "You must provid your First Name")]
        [Display(Name = "First Name")]
        public string FirstName{get; set;}

        [Required(ErrorMessage = "You must provide your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName{get; set;}

        [Required(ErrorMessage = "You must provide your Birth Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public string BirthDate{get; set;}

        [Required]
        [Display(Name = "Gender")]
        public string Gender{get; set;}

        [Required(ErrorMessage = "You must provide a PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public int PhoneNumber{get; set;}
    }

    public class ForgotPasswordViewModel {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email{get; set;}
    }

    public class AccountViewModels {
        [Required(ErrorMessage = "You must provide your old password to save changes")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string Password { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "You must provid your First Name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must provide your Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must provide your Birth Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "You must provide a PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public int PhoneNumber { get; set; }
    }

}