using System.Collections.Generic;
using TicketManagement.Helpers;
using TicketManagement.Models.Entities;

namespace TicketManagement.ViewModels
{
    class TicketViewModels
    {
    }

    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }

        public List<TicketLog> TicketLogs { get; set; } 
    }

    public class NewTicketLogViewModel
    {
        public int TicketId { get; set; }

        public string Message { get; set; }

        public bool SendSms { get; set; }

        public bool IsInternal { get; set; }

        public bool CloseOnReply { get; set; }
    }
}