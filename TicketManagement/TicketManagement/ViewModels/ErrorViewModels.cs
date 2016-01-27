using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Management;

namespace TicketManagement.ViewModels
{
    class ErrorViewModels
    {
    }

    public class ErrorViewModel
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
    }
}
