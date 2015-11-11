using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class TicketLog
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        //[Required]
        //[ForeignKey("Ticket")]
        //[DisplayName("Ticket")]
        //public int TicketId { get; set; }

        virtual public Ticket Ticket { get; set; }

        //[Required]
        //[ForeignKey("TicketLogType")]
        //[DisplayName("Ticket Log Type")]
        //public int TicketLogTypeId { get; set; }

        virtual public TicketLogType TicketLogType { get; set; }

        [Required]
        [DisplayName("Log Data")]
        public string Data { get; set; }

        [Required]
        [DisplayName("Time of Log")]
        public DateTime TimeOfLog { get; set; } = DateTime.Now;
    }
}
