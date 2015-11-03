using TicketManagement.Models.Entities;

namespace TicketManagement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketManagement.Models.Context.TicketManagementContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TicketManagement.Models.Context.TicketManagementContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Organisations.AddOrUpdate( o => o.Name,
                new Organisation { Name = "My Company", IsInternal = true, Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Organisation { Name = "Client", IsInternal = false, Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.Teams.AddOrUpdate(t => t.Name,
                new Team { Name = "Support", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Team { Name = "Management", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.Projects.AddOrUpdate( p => p.Name,
                new Project { Name = "First Application", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Project { Name = "Second Application", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.Users.AddOrUpdate(u => u.FirstName,
                new User { FirstName = "John", LastName = "Doe", UserName = "JohnDoe", Email = "johndoe@example.com", Telephone = "01234 987654",
                    IsAdmin = true, IsInternal = true, IsArchived = false, IsTeamLeader = false, Created = DateTime.Now, LastUpdated = DateTime.Now },
                new User { FirstName = "Jane", LastName = "Doe", UserName = "JaneDoe", Email = "janedoe@example.com", Telephone = "01234 987654",
                    IsAdmin = false, IsInternal = false, IsArchived = false, IsTeamLeader = false, Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketCategories.AddOrUpdate(t => t.Name,
                new TicketCategory { Name = "Question", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketCategory { Name = "Bug", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketCategory { Name = "Feature", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketLogTypes.AddOrUpdate(t => t.Name,
                new TicketLogType { Name = "Message", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketLogType { Name = "Message", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketPriorities.AddOrUpdate(t => t.Name,
                new TicketPriority { Name = "Feature", Colour = "#0066FF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Low", Colour = "#00CC00", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Medium", Colour = "#FF6600", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "High", Colour = "#FF0000", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Emergency", Colour = "#800000", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TickerStates.AddOrUpdate(ts => ts.Name,
                new TicketState { Name = "Pending Approval", Colour = "#009900", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Open", Colour = "#CCFFFF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Awaiting Response", Colour = "#FFFFFF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Closed", Colour = "#FF0000", Created = DateTime.Now, LastUpdated = DateTime.Now });
        }
    }
}
