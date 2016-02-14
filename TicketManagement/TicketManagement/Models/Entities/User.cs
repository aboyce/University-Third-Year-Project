using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketManagement.Models.Entities
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public User()
        {
        }

        public User(string email, string firstName, string lastName, string userName, string phoneNumber, bool isArchived = false)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            IsArchived = isArchived;
        }

        private string _firstName;
        private string _lastName;
        private bool _isArchived = false;
        private int? _teamId = null;
        private Team _team = null;
        private bool _isTeamLeader = false;
        private bool _mobileApplicationConfirmed = false;
        private string _userToken;

        [DisplayName("Name")]
        public string FullName => $"{_firstName} {_lastName}";

        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        [Required]
        [DisplayName("Mobile Application Confirmed")]
        public bool MobileApplicationConfirmed
        {
            get { return _mobileApplicationConfirmed; }
            set { _mobileApplicationConfirmed = value; }
        }

        [StringLength(100, MinimumLength = 5)]
        [DisplayName("User Token")]
        public string UserToken
        {
            get { return _userToken; }
            set { _userToken = value; }
        }

        [Required]
        [DisplayName("Archived")]
        public bool IsArchived
        {
            get { return _isArchived; }
            set { _isArchived = value; }
        }

        [ForeignKey("Team")]
        [DisplayName("Team")]
        public int? TeamId
        {
            get { return _teamId; }
            set { _teamId = value; }
        }

        public virtual Team Team
        {
            get { return _team; }
            set { _team = value; }
        }

        [DisplayName("Is Team Leader")]
        public bool IsTeamLeader
        {
            get { return _isTeamLeader; }
            set { _isTeamLeader = value; }
        }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

    }
}
