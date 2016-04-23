using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;
using TicketManagement.ViewModels;

namespace TicketManagement.Helpers
{
    public static class ApiHelper
    {

        public static Task<ApiTicketViewModel> GetApiTicketViewModelAsync(Ticket ticket, string colour)
        {
            return Task.Factory.StartNew(() => GetApiTicketViewModel(ticket, colour));
        }
        public static ApiTicketViewModel GetApiTicketViewModel(Ticket ticket, string colour)
        {
            if (ticket == null)
                return null;

            return new ApiTicketViewModel
            {
                Id = ticket.Id,
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
                Colour = colour,
                Deadline = ticket.Deadline,
                LastMessage = ticket.LastMessage,
                LastResponse = ticket.LastResponse
            };
        }

        public static Task<ApiTicketLogViewModel> GetApiTicketLogViewModelAsync(TicketLog ticketLog)
        {
            return Task.Factory.StartNew(() => GetApiTicketLogViewModel(ticketLog));
        }
        public static ApiTicketLogViewModel GetApiTicketLogViewModel(TicketLog ticketLog)
        {
            if (ticketLog == null)
                return null;

            return new ApiTicketLogViewModel
            {
                Id = ticketLog.Id,
                TicketId = ticketLog.TicketId,
                SubmittedByName = ticketLog.SubmittedByUser.FullName,
                HasFile = ticketLog.FileId != null,
                IsInternal = ticketLog.IsInternal,
                FromInternal = ticketLog.TicketLogType == TicketLogType.MessageFromInternalUser,
                Message = ticketLog.Message,
                TimeOfLog = ticketLog.TimeOfLog
            };
        }
    }
}
