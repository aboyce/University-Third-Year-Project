using System.ComponentModel.DataAnnotations;
using TicketManagement.Models.Entities;

namespace TicketManagement.ViewModels
{
    class HomeViewModels
    {
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Internal User")]
        public bool IsInternal { get; set; } = false;

        [Required]
        [Display(Name = "Archived")]
        public bool IsArchived { get; set; } = false;

        public Team Team { get; set; } = null;

        [Display(Name = "Is Team Leader")]
        public bool IsTeamLeader { get; set; } = false;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}