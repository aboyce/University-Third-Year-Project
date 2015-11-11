using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext()
            : base("TicketManagement", throwIfV1Schema: false)
        {
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User").HasKey(u => u.Id);
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole").HasKey(r => r.UserId);
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin").HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim").HasKey(c => c.Id);
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").HasKey(r => r.Id);
        }
    }
}
