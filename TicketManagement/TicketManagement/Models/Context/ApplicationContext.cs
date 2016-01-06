using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketManagement.Management;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Context
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext()
        //: base("TicketManagement", throwIfV1Schema: false)
        : base("TicketManagementLocal", throwIfV1Schema: false)
        //: base("TicketManagementGoDaddy", throwIfV1Schema: false)
        {
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<TicketLog> TicketLogs { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<TicketPriority> TicketPriorities { get; set; }
        public DbSet<TicketState> TicketStates { get; set; }
        public DbSet<SentTextMessage> TextMessagesSent { get; set; }
        public DbSet<ReceivedTextMessage> TextMessagesReceived { get; set; }

        public DbSet<RoleNotification> RoleNotifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        }

        //public System.Data.Entity.DbSet<TicketManagement.Models.Entities.User> ApplicationUsers { get; set; }
    }
}
