using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.ViewModels
{
    class ApiViewModels
    {
    }

    public class ApiUserTokenViewModel
    {
        public string UserToken { get; set; }
        public bool IsInternal { get; set; }
    }

    public class ApiTicketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OpenedByName { get; set; }
        public string  TicketPriorityName { get; set; }
        public string UserAssignedToName { get; set; }
        public string TeamAssignedToName { get; set; }
        public string OrganisationAssignedToName { get; set; }
        public string TicketStateName { get; set; }
        public string ProjectName { get; set; }
        public string TicketCategoryName { get; set; }
        public string Colour { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? LastMessage { get; set; }
        public DateTime? LastResponse { get; set; }
    }

    public class ApiTicketLogViewModel
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string SubmittedByName { get; set; }
        public bool HasFile { get; set; }
        public bool IsInternal { get; set; }
        public bool FromInternal { get; set; }
        public string Message { get; set; }
        public DateTime? TimeOfLog { get; set; }
    }
}
