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
    class Organisation
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Is Internal")]
        public bool IsInternal { get; set; }

        [ForeignKey("Default Contact")]
        [DisplayName("Default Contact")]
        public int? DefaultContactId { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
