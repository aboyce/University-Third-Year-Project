using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Context
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext()
            : base("TicketManagement", throwIfV1Schema: false)
        {
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketLog> TicketLogs { get; set; }
        public DbSet<TicketLogType> TicketLogTypes { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketState> TicketStates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}
