using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Helpers;

namespace TicketManagement.Models.Entities
{
    public class SentTextMessage : Base_TextMessage
    {
        public SentTextMessage(string userToId, User userTo, string to, string content)
        {
            UserToId = userToId;
            UserTo = userTo;
            To = to;
            Content = content;

        }

        [Required]
        public string UserToId { get; set; }

        [Required]
        public User UserTo { get; set; }

        [Required]
        public new string From { get; } = ConfigurationHelper.GetTextMessageFromCode();

        [Required]
        public DateTime Sent { get; set; } = DateTime.Now;
    }
}
