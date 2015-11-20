using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.ComponentModel.DataAnnotations;
using TicketManagement.Models.Entities;

namespace TicketManagement.ViewModels
{
    class AdminViewModels
    {
    }

    public class ManageUsersViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Username must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name ="Username")]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Internal User")]
        public bool IsInternal { get; set; } = false;

        [Display(Name = "Admin User")]
        public bool IsAdmin { get; set; } = false;

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; } = false;

        public Team Team { get; set; } = null;

        [Display(Name = "Is Team Leader")]
        public bool IsTeamLeader { get; set; } = false;
    }

    public class ExternalLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    //
    //POSSIBLE, DEPENDING ON USE OF 3RD PARTY LOGIN
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
}
