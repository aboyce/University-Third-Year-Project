using System.Data.Common;
using System.Data.Entity.Migrations;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Management;
using TicketManagement.Models.Context;
using TicketManagement.Models.Entities;
using File = System.IO.File;

namespace TicketManagement.Tests
{
    [TestClass]
    public class TestBase
    {
        private ApplicationContext _db;
        private string currentFile;

        public ApplicationContext Database => _db;

        [TestInitialize]
        public void Initialise()
        {
            currentFile = Path.GetTempFileName();

            if (string.IsNullOrEmpty(currentFile))
                return;

            string connectionString = $"Data Source={currentFile};Persist Security Info=False";

            DbConnection connection = DbProviderFactories.GetFactory("System.Data.SqlServerCe.4.0").CreateConnection();

            if(connection == null)
                return;

            connection.ConnectionString = connectionString;
            connection.Open();

            _db = new ApplicationContext(connection);
            _db.Database.CreateIfNotExists();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _db.Dispose();
            _db = null;
            File.Delete(currentFile);
        }

        protected virtual void SeedDatabase()
        {
            if (!new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db)).RoleExists(MyRoles.Administrator))
                _db.Roles.AddOrUpdate(
                new IdentityRole(MyRoles.Approved),
                new IdentityRole(MyRoles.Internal),
                new IdentityRole(MyRoles.Social),
                new IdentityRole(MyRoles.TextMessage),
                new IdentityRole(MyRoles.Administrator));

            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(Database));

            User internalUser = new User
            {
                FirstName = "Internal",
                LastName = "Internal",
                UserName = "Internal",
                Email = "internal@email.com",
                PhoneNumber = "00000000000",
                IsArchived = false
            };

            userManager.Create(internalUser, "randomlyGeneratedPassword");
            userManager.AddToRoles(internalUser.Id, MyRoles.Internal);

            User nonInternalUser = new User
            {
                FirstName = "NonInternal",
                LastName = "NonInternal",
                UserName = "NonInternal",
                Email = "non_internal@email.com",
                PhoneNumber = "00000000000",
                IsArchived = false
            };

            userManager.Create(nonInternalUser, "randomlyGeneratedPassword");
        }
    }
}
