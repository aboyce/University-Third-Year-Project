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
    class TicketLog
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [Required]
        [ForeignKey("TicketLogType")]
        [DisplayName("Ticket Log Type")]
        public int TicketLogTypeId { get; set; }

        [Required]
        [DisplayName("Log Data")]
        public string Data { get; set; }

        [Required]
        [DisplayName("Time of Log")]
        public DateTime TimeOfLog { get; set; } = DateTime.Now;
    }
}
