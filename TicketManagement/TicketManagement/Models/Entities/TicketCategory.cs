using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class TicketCategory
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Category Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("Ticket Category Name")]
        public string Name { get; set; }

        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int? ProjectId { get; set; }

        virtual public Project Project { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
