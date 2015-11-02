using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Project
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        public string Name { get; set; }

        [ForeignKey("Organisation")]
        [DisplayName("Organisation")]
        public int? OrganisationId { get; set; }

        virtual public Organisation Organisation { get; set; }

        [ForeignKey("TeamAssignedTo")]
        [DisplayName("Team Assigned To")]
        public int? TeamAssignedToId { get; set; }

        virtual public Team TeamAssignedTo { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
