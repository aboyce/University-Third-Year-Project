using TicketManagement.Models.Entities;

namespace TicketManagement.Migrations.Entities
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
            MigrationsDirectory = @"Migrations\Entities";
        }

        protected override void Seed(TicketManagement.Models.Context.TicketManagementContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.Organisations.AddOrUpdate(
                new Organisation { Name = "My Company", IsInternal = true, ContactUserId = null, DefaultContact = null, Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Organisation { Name = "Client", IsInternal = false, ContactUserId = null, DefaultContact = null, Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.Teams.AddOrUpdate(
                new Team { Name = "Support", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Team { Name = "Management", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.Projects.AddOrUpdate(
                new Project { Name = "First Application", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new Project { Name = "Second Application", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketCategories.AddOrUpdate(
                new TicketCategory { Name = "Question", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketCategory { Name = "Bug", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketCategory { Name = "Feature", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketLogTypes.AddOrUpdate(
                new TicketLogType { Name = "Message", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketLogType { Name = "Message", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketPriorities.AddOrUpdate(
                new TicketPriority { Name = "Feature", Colour = "#0066FF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Low", Colour = "#00CC00", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Medium", Colour = "#FF6600", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "High", Colour = "#FF0000", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketPriority { Name = "Emergency", Colour = "#800000", Created = DateTime.Now, LastUpdated = DateTime.Now });

            context.TicketStates.AddOrUpdate(
                new TicketState { Name = "Pending Approval", Colour = "#009900", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Open", Colour = "#CCFFFF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Awaiting Response", Colour = "#FFFFFF", Created = DateTime.Now, LastUpdated = DateTime.Now },
                new TicketState { Name = "Closed", Colour = "#FF0000", Created = DateTime.Now, LastUpdated = DateTime.Now });
        }
    }
}
