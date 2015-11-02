using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    class Ticket
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title must be less that 100 characters but more than 5", MinimumLength = 5)]
        public string Title { get; set; }

        [StringLength(250, ErrorMessage = "Description must be less that 250 characters but more than 5", MinimumLength = 5)]
        public string Description { get; set; }

        [Required]
        [ForeignKey("OpenedBy")]
        [DisplayName("Opened By")]
        public int OpenedById { get; set; }

        [Required]
        [ForeignKey("TicketPriority")]
        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; }

        [Required]
        [ForeignKey("TicketPriorityInternal")]
        [DisplayName("Ticket Priority Internally")]
        public int TicketPriorityInternalId { get; set; }

        [ForeignKey("UserAssignedTo")]
        [DisplayName("User Assigned To")]
        public int? UserAssignedToId { get; set; }

        [ForeignKey("TeamAssignedTo")]
        [DisplayName("Team Assigned To")]
        public int? TeamAssignedToId { get; set; }

        [ForeignKey("OrganisationAssignedTo")]
        [DisplayName("Organisation Assigned To")]
        public int? OrganisationAssignedToId { get; set; }

        [Required]
        [ForeignKey("TicketState")]
        [DisplayName("Ticket State")]
        public int TicketStateId { get; set; }

        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int? ProjectId { get; set; }

        [Required]
        [ForeignKey("TicketCategory")]
        [DisplayName("Ticket Category")]
        public int TicketCategoryId { get; set; }

        public DateTime? Deadline { get; set; }

        [DisplayName("Last Message")]
        public DateTime? LastMessage { get; set; }

        [DisplayName("Last Response")]
        public DateTime? LastResponse { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
