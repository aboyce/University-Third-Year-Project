using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketManagement.Models.Entities
{
    public class TextMessage
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public DateTime Sent { get; set; } = DateTime.Now;

        [Required]
        public string To { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string Message { get; set; }
    }
}