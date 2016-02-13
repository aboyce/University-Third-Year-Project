using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Helpers
{
    public static class ApiHelper
    {

        public static Task<ApiTicketViewModel> GetApiTicketViewModelAsync(Ticket ticket)
        {
            return Task.Factory.StartNew(() => GetApiTicketViewModel(ticket));
        }
        public static ApiTicketViewModel GetApiTicketViewModel(Ticket ticket)
        {
            if (ticket == null)
                return null;

            return new ApiTicketViewModel
            {
                Title = ticket.Description,
                Description = ticket.Description
            };
        }
    }
}
