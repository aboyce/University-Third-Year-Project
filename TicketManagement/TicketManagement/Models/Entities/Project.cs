using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Project
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        public string Name { get; set; }

        //[ForeignKey("Organisation")]
        //[DisplayName("Organisation")]
        //public int? OrganisationId { get; set; } = null;

        virtual public Organisation Organisation { get; set; } = null;

        //[ForeignKey("TeamAssignedTo")]
        //[DisplayName("Team Assigned To")]
        //public int? TeamAssignedToId { get; set; } = null;

        virtual public Team TeamAssignedTo { get; set; } = null;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public virtual ICollection<TicketCategory> TicketCategories { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
