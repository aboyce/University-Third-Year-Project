using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    class TicketPriority
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Priority Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        [DisplayName("Ticket Priority Name")]
        public string Name { get; set; }

        [StringLength(10, ErrorMessage = "Username must be less that 10 characters but more than 3", MinimumLength = 3)]
        public string Colour { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
