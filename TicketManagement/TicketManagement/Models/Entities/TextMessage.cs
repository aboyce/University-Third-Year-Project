using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketManagement.Models.Entities
{
    public class TextMessage
    {
        public TextMessage() { }

        public TextMessage(string userToId, ApplicationUser userTo, string number, string message)
        {
            ApplicationUserToId = userToId;
            ApplicationUserTo = userTo;
            Number = number;
            Message = message;
        }

        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public string ApplicationUserToId { get; set; }

        [Required]
        public ApplicationUser ApplicationUserTo { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string From { get; } = Helpers.ConfigurationHelper.GetTextMessageFromCode();

        [Required]
        public DateTime Sent { get; set; } = DateTime.Now;
    }
}