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
            if(await controllerUnderTest.GetNewUserToken(user.UserName) != null)
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

        [TestMethod]
        public async Task Test_UserController_CheckUserToken()
        {
            // Setup.
            SeedDatabase();
            UserController controllerUnderTest = new UserController(Database);

            // Check that the controller handles incorrect paramters.
            // These should be returning 'false'.
            if (await controllerUnderTest.CheckUserToken(null, null))
                Assert.Fail("Controller not handling incorrect parameters.");
            if (await controllerUnderTest.CheckUserToken("", ""))
                Assert.Fail("Controller not handling incorrect parameters.");
            // Test it can handle incorrect Users as parameters
            if (await controllerUnderTest.CheckUserToken("this_is_not_a_username_that_should_be_in_the_db", "this_is_not_a_usertoken_that_should_be_in_the_db"))
                Assert.Fail("Controller not handling incorrect parameters.");

            // Check that we are set up as expected for the test.
            List<User> users = Database.Users.ToList();
            if (users.Count != 2)
                Assert.Fail("Seeded database not as expected for this test.");

            // Setup (A User does not have a mobile setup and has a new, valid User Token.).
            User user = users.First();
            user.MobileApplicationConfirmed = false;
            await controllerUnderTest.GetNewUserToken(user.UserName); // This should work, as we have independently tested it.
            Database.Entry(user).State = EntityState.Modified;
            await Database.SaveChangesAsync();

            // Test that controller doesn't give us back an incorrect response (with the MobileApplicationConfirmed).
            if (await controllerUnderTest.CheckUserToken(user.UserName, user.UserToken))
                Assert.Fail("The controller should not have said that the token was correct, as the MobileApplicaitonConfirmed should be false.");

            // Setup.
            user.MobileApplicationConfirmed = true;
            Database.Entry(user).State = EntityState.Modified;
            await Database.SaveChangesAsync();

            // Test that the controller prevents us from continuing when the User is not supposed to be used.
            if (!await controllerUnderTest.CheckUserToken(user.UserName, user.UserToken))
                Assert.Fail("Controller should have confirmed that the passed in User Token is valid.");
        }




        protected override void SeedDatabase()
        {
            base.SeedDatabase();
        }
    }
}
