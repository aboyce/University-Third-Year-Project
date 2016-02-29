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
            To = to ?? "n/a";
            From = from ?? "n/a";
            Content = content ?? "n/a";
            ClockworkId = clockworkId ?? "n/a";
            ClockworkNetworkCode = clockworkNetworkCode ?? "n/a";
            ClockworkKeyword = clockworkKeyword ?? "n/a";
        }


        [Required]
        public string ClockworkNetworkCode { get; set; }

        [Required]
        public string ClockworkKeyword { get; set; }

        public string UserFromId { get; set; }

        public User UserFrom { get; set; }

        [Required]
        public bool Read { get; set; } = false;

        [Required]
        public DateTime Received { get; set; } = DateTime.Now;
    }
}
