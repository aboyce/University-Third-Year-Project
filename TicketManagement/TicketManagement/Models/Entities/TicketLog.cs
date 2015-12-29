using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketManagement.Helpers;

namespace TicketManagement.Models.Entities
{
    public class TicketLog
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        virtual public Ticket Ticket { get; set; }

        [ForeignKey("SubmittedByUser")]
        public string SubmittedByUserId { get; set; }

        virtual public User SubmittedByUser { get; set; }

        [Required]
        [DisplayName("Ticket Log Type")]
        public TicketLogType TicketLogType { get; set; }

        [ForeignKey("File")]
        public int? FileId { get; set; } = null;

        public virtual File File { get; set; } = null;

        public string Message { get; set; }

        [Required]
        [DisplayName("Is Internal")]
        public bool IsInternal { get; set; }

        [Required]
        [DisplayName("Time of Log")]
        public DateTime TimeOfLog { get; set; } = DateTime.Now;
    }
}
