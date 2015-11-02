using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Team
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Team Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("Team Name")]
        public string Name { get; set; }

        [ForeignKey("Organisation")]
        [DisplayName("Organisation")]
        public int? OrganisationId { get; set; }

        virtual public Organisation Organisation { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
