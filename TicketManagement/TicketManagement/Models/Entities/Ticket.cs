using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Ticket
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title must be less that 100 characters but more than 2", MinimumLength = 2)]
        public string Title { get; set; }

        [StringLength(250, ErrorMessage = "Description must be less that 250 characters but more than 2", MinimumLength = 2)]
        public string Description { get; set; }

        //[Required]
        //[ForeignKey("OpenedBy")]
        //[DisplayName("Opened By")]
        //public int OpenedById { get; set; }

        virtual public User OpenedBy { get; set; }

        //[Required]
        //[ForeignKey("TicketPriority")]
        //[DisplayName("Ticket Priority")]
        //public int TicketPriorityId { get; set; }

        virtual public TicketPriority TicketPriority { get; set; }

        //[Required]
        //[ForeignKey("TicketPriorityInternal")]
        //[DisplayName("Ticket Priority Internally")]
        //public int TicketPriorityInternalId { get; set; }

        //virtual public TicketPriority TicketPriorityInternal { get; set; }

        //[ForeignKey("UserAssignedTo")]
        //[DisplayName("User Assigned To")]
        //public int? UserAssignedToId { get; set; } = null;

        virtual public User UserAssignedTo { get; set; } = null;

        //[ForeignKey("TeamAssignedTo")]
        //[DisplayName("Team Assigned To")]
        //public int? TeamAssignedToId { get; set; } = null;

        virtual public Team TeamAssignedTo { get; set; } = null;

        //[ForeignKey("OrganisationAssignedTo")]
        //[DisplayName("Organisation Assigned To")]
        //public int? OrganisationAssignedToId { get; set; } = null;

        virtual public Organisation OrganisationAssignedTo { get; set; } = null;

        //[Required]
        //[ForeignKey("TicketState")]
        //[DisplayName("Ticket State")]
        //public int TicketStateId { get; set; }

        virtual public TicketState TicketState { get; set; }

        //[ForeignKey("Project")]
        //[DisplayName("Project")]
        //public int? ProjectId { get; set; } = null;

        virtual public Project Project { get; set; } = null;

        //[Required]
        //[ForeignKey("TicketCategory")]
        //[DisplayName("Ticket Category")]
        //public int TicketCategoryId { get; set; }

        virtual public TicketCategory TicketCategory { get; set; }

        public DateTime? Deadline { get; set; } = null;

        [DisplayName("Last Message")]
        public DateTime? LastMessage { get; set; } = null;

        [DisplayName("Last Response")]
        public DateTime? LastResponse { get; set; } = null;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public virtual ICollection<TicketLog> TicketLogs { get; set; }
    }
}
