using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Controllers.API;
using System.Linq;
using TicketManagement.Management;
using TicketManagement.Models.Entities;

namespace TicketManagement.Tests.Controllers.API
{
    [TestClass]
    public class BaseApiControllerTests : TestBase
    {
        [TestMethod]
        public void Test_BaseApiController_GetDefault()
        {
            BaseApiController controller = new BaseApiController(Database);
            string response = controller.Get();
            if (string.IsNullOrEmpty(response))
                Assert.Fail("No response from Controller.");
        }

        [TestMethod]
        public void Test_BaseApiController_GetWithId()
        {
            int idParameter = 7;
            BaseApiController controller = new BaseApiController(Database);
            string response = controller.Get(idParameter);
            if (!response.Contains(idParameter.ToString()))
                Assert.Fail("Controller failed to incorperate the parameter in the response.");
        }

        [TestMethod]
        public void Test_BaseApiController_CheckConnection()
        {
            BaseApiController controller = new BaseApiController(Database);
            if (!controller.CheckConnection())
                Assert.Fail("Controller failed to respond correctly.");
        }

        [TestMethod]
        public void Test_BaseApiController_IsUserInternal()
        {
            SeedDatabase();
            BaseApiController controller = new BaseApiController(Database);
            List<User> users = Database.Users.ToList(); // Get all the Users from the seeded database.
            if(users.Count != 2)
                Assert.Fail("Seeded database not as expected for this test.");

            string temp = "";



        }

        protected override void SeedDatabase()
        {
            base.SeedDatabase();

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
