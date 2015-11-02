﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    class User
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("Username")]
        public string UserName { get; set; }

        [StringLength(50, ErrorMessage = "Email must be less that 50 characters but more than 5", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Telephone must be less that 50 characters but more than 5", MinimumLength = 5)]
        [Phone]
        public string Telephone { get; set; }

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
        public int? TeamId { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; }
    }
}
