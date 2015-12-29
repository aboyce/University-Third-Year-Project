using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;

namespace TicketManagement.ViewModels
{
    class TicketStateViewModels
    {
    }

    public class EditTicketStateViewModel
    {
        public bool IsEditable { get; set; }
        public TicketState TicketState { get; set; }
    }
}
