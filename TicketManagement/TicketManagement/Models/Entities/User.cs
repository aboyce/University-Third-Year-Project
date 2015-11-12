using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class User
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        //[Required]
        [DisplayName("Application User")]
        public string ApplicationUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "Email must be less that 50 characters but more than 2", MinimumLength = 2)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [StringLength(50, ErrorMessage = "Telephone must be less that 50 characters but more than 2", MinimumLength = 2)]
        [Phone]
        public string Telephone { get; set; } = "";

        [Required]
        [DisplayName("Internal User")]
        public bool IsInternal { get; set; } = false;

        [Required]
        [DisplayName("Admin User")]
        public bool IsAdmin { get; set; } = false;

        [Required]
        [DisplayName("Archived")]
        public bool IsArchived { get; set; } = false;

        [ForeignKey("Team")]
        [DisplayName("Team")]
        public int? TeamId { get; set; } = null;

        virtual public Team Team { get; set; } = null;

        [DisplayName("Is Team Leader")]
        public bool IsTeamLeader { get; set; } = false;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
