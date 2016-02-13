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
                Title = ticket.Title,
                Description = ticket.Description,
                OpenedByName = ticket.OpenedBy?.FullName,
                TicketPriorityName = ticket.TicketPriority?.Name,
                UserAssignedToName = ticket.UserAssignedTo?.FullName,
                TeamAssignedToName = ticket.TeamAssignedTo?.Name,
                ProjectName = ticket.Project?.Name,
                OrganisationAssignedToName = ticket.OrganisationAssignedTo?.Name,
                TicketStateName = ticket.TicketState?.Name,
                TicketCategoryName = ticket.TicketCategory?.Name,
                Deadline = ticket.Deadline,
                LastMessage = ticket.LastMessage,
                LastResponse = ticket.LastResponse
            };
        }
    }
}
