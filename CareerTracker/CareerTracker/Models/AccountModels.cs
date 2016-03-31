using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CareerTracker.Models
{
    /*
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string email { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Active Account")]
        public bool active { get; set; }
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public string dateOfBirth { get; set; }

        public virtual ICollection<Goal> goals { get; set; }
        public virtual ICollection<Artifact> artifacts { get; set; }
        public virtual ICollection<Skill> skills { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
    */

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Alphanumeric("The password must have at least 1 number and 1 letter.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    public class AlphanumericAttribute : ValidationAttribute
    {
        public AlphanumericAttribute(string errorMessage)
            : base(errorMessage)
        { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            string pw = value.ToString();

            bool num = false;
            bool letter = false;
            foreach (char c in pw)
            {
                if (Char.IsDigit(c))
                {
                    num = true;
                }
                else
                {
                    if (Char.IsLetter(c))
                    {
                        letter = true;
                    }
                }
            }
            if (!num || !letter)
            {
                validationResult = new ValidationResult(ErrorMessageString);
            }
            return validationResult;
        }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
