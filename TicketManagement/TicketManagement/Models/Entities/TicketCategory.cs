using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class TicketCategory
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Category Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Ticket Category Name")]
        public string Name { get; set; }

        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int? ProjectId { get; set; } = null;

        public virtual Project Project { get; set; } = null;

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
