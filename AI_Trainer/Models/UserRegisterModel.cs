using System.ComponentModel.DataAnnotations;
using System;

namespace AI_Trainer.Models
{
    public class UserRegister
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be longer than 2 characters")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Name must be longer than 2 characters")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be longer than 8 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "Passwords do not Match.")]
        public string PasswordCF { get; set; }
    }
}