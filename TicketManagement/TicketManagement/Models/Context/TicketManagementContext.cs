using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Context
{
    public class TicketManagementContext : DbContext
    {

        public TicketManagementContext() : base("TicketManagement") {}

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketCategory> TicketCategories  { get; set; }
        public DbSet<TicketLog> TicketLogs { get; set; }
        public DbSet<TicketLogType> TicketLogTypes { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketState> TicketStates { get; set; }
    }
}
