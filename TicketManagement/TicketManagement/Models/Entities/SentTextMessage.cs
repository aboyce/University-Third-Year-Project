using System;
using System.ComponentModel.DataAnnotations;
using TicketManagement.Helpers;

namespace TicketManagement.Models.Entities
{
    public class SentTextMessage : Base_TextMessage
    {
        public SentTextMessage() {}

        public SentTextMessage(string userToId, User userTo, string to, string content, bool success = false)
        {
            UserToId = userToId;
            UserTo = userTo;
            To = to;
            Content = content;
            Success = success;
        }

        [Required]
        public string UserToId { get; set; }

        [Required]
        public User UserTo { get; set; }

        [Required]
        public new string From { get; } = ConfigurationHelper.GetTextMessageFromCode();

        [Required]
        public bool Success { get; set; } = false;

        public string DeliveryStatus { get; set; }

        public string DeliveryDetail { get; set; }

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        [Required]
        public DateTime Sent { get; set; } = DateTime.Now;
    }
}
