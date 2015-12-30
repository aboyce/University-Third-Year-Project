using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    public class ReceivedTextMessage : Base_TextMessage
    {
        [Required]
        public string ClockworkId { get; set; }

        [Required]
        public bool Read { get; set; }

        [Required]
        public DateTime Received { get; set; } = DateTime.Now;
    }
}
