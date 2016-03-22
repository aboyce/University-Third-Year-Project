using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Controllers.API;
using TicketManagement.Models.Entities;

namespace TicketManagement.Tests.Controllers.API
{
    [TestClass]
    public class UserControllerTests : TestBase
    {
        [TestMethod]
        public async Task Test_UserController_GetNewUserToken()
        {
            // Setup.
            SeedDatabase();
            UserController controllerUnderTest = new UserController(Database);

            // Check that the controller handles incorrect paramters.
            if(await controllerUnderTest.GetNewUserToken(null) != null)
                Assert.Fail("Controller not handling incorrect parameters.");
            if (await controllerUnderTest.GetNewUserToken("") != null)
                Assert.Fail("Controller not handling incorrect parameters.");
            // Test it can handle incorrect Users as parameters
            if (await controllerUnderTest.GetNewUserToken("this_is_not_a_username_that_should_be_in_the_db") != null)
                Assert.Fail("Controller not handling incorrect parameters.");

            // Check that we are set up as expected for the test.
            List<User> users = Database.Users.ToList();
            if (users.Count != 2)
                Assert.Fail("Seeded database not as expected for this test.");

            // Setup.
            User user = users.First();
            user.MobileApplicationConfirmed = true;
            Database.Entry(user).State = EntityState.Modified;
            await Database.SaveChangesAsync();

            // Test that the controller prevents us from continuing when the User is not supposed to be used.
            if(await controllerUnderTest.GetNewUserToken(user.Id) != null)
                Assert.Fail("Controller did not prevent progression when the User was not in the correct state.");

            // Setup.
            user.MobileApplicationConfirmed = false;
            Database.Entry(user).State = EntityState.Modified;
            await Database.SaveChangesAsync();

           // Test that the controller gives us back a result when the environment is correct.
           JsonResult result = await controllerUnderTest.GetNewUserToken(user.UserName);
            if(result == null)
                Assert.Fail("Json Result is null");
            if (result.ContentType != "UserTokenAndIsInternal")
                Assert.Fail("JsonResult ContentType not as expected.");
            if (result.Data == null)
                Assert.Fail("JsonResult Data not as expected.");
        }




        protected override void SeedDatabase()
        {
            base.SeedDatabase();
        }
    }
}
