using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public class ReceivedTextMessage : Base_TextMessage
    {
        public ReceivedTextMessage() {}

        public ReceivedTextMessage(string to, string from, string content, string clockworkId,
            string clockworkNetworkCode, string clockworkKeyword)
        {
            To = to;
            From = from;
            Content = content;
            ClockworkId = clockworkId;
            ClockworkNetworkCode = clockworkNetworkCode;
            ClockworkKeyword = clockworkKeyword;
        }

        [Required]
        public string ClockworkId { get; set; }

        [Required]
        public string ClockworkNetworkCode { get; set; }

        [Required]
        public string ClockworkKeyword { get; set; }

        [Required]
        public bool Read { get; set; } = false;

        [Required]
        public DateTime Received { get; set; } = DateTime.Now;
    }
}
