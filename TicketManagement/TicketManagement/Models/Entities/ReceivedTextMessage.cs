using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public class ReceivedTextMessage : Base_TextMessage
    {
        public ReceivedTextMessage() {}

        [Required]
        public string ClockworkId { get; set; }

        [Required]
        public string ClockworkNetworkCode { get; set; }

        [Required]
        public string ClockworkNetworkKeyword { get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public DateTime Received { get; set; } = DateTime.Now;
    }
}
