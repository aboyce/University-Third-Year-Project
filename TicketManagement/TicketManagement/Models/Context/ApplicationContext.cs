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

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUser");
        //    modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole").HasKey(ur => ur.UserId);
        //    modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin").HasKey(ul => ul.UserId);
        //    modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
        //    modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        //}
    }
}
