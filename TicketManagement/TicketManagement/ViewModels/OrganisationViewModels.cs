using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;

namespace TicketManagement.ViewModels
{
    class OrganisationViewModels
    {
    }

    public class OrganisationsStructureViewModel
    {
        public OrganisationsStructureViewModel()
        {
            Organisations = new List<OrganisationTeamsViewModel>();
        }

        public List<OrganisationTeamsViewModel> Organisations { get; set; }
    }

    public class OrganisationTeamsViewModel
    {
        public OrganisationTeamsViewModel()
        {
            TeamsForOrganisations = new List<ProjectsUsersForTeamViewModel>();
        }

        public Organisation Organisation { get; set; }

        public List<ProjectsUsersForTeamViewModel> TeamsForOrganisations { get; set; }
    }

    public class ProjectsUsersForTeamViewModel
    {
        public ProjectsUsersForTeamViewModel()
        {
            ProjectsForTeams = new List<Project>();
            UsersForTeams = new List<User>();
        }

        public Team Team { get; set; }

        public List<Project> ProjectsForTeams { get; set; }

        public List<User> UsersForTeams { get; set; }
    }
}
