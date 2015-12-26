using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;
using TicketManagement.Models.Management;

namespace TicketManagement.ViewModels
{
    class TicketViewModels
    {
    }

    public class NewTicketLogViewModel
    {
        public int TicketId { get; set; }

        public TicketLogType TicketLogType { get; set; }

        public string Data { get; set; }
    }
}