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
        public DateTime? Deadline { get; set; }
        public DateTime? LastMessage { get; set; }
        public DateTime? LastResponse { get; set; }
    }
}
