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

        [TestMethod]
        public async Task Test_UserController_DeactivateUserToken()
        {
            // Setup.
            SeedDatabase();
            UserController controllerUnderTest = new UserController(Database);

            // Check that the controller handles incorrect paramters.
            // These should be returning 'false'.
            if (await controllerUnderTest.DeactivateUserToken(null, null))
                Assert.Fail("Controller not handling incorrect parameters.");
            if (await controllerUnderTest.DeactivateUserToken("", ""))
                Assert.Fail("Controller not handling incorrect parameters.");
            // Test it can handle incorrect Users as parameters
            if (await controllerUnderTest.DeactivateUserToken("this_is_not_a_username_that_should_be_in_the_db", "this_is_not_a_usertoken_that_should_be_in_the_db"))
                Assert.Fail("Controller not handling incorrect parameters.");

            // Check that we are set up as expected for the test.
            List<User> users = Database.Users.ToList();
            if (users.Count != 2)
                Assert.Fail("Seeded database not as expected for this test.");

            // Setup (Create the state that a User would be in if they had a User Token that was activated.).
            User user = users.First();

            string userId = user.Id; // Used to get the User back out later on.
            string userUserToken = Guid.NewGuid().ToString();

            user.UserToken = userUserToken;
            user.MobileApplicationConfirmed = true;
            Database.Entry(user).State = EntityState.Modified;
            await Database.SaveChangesAsync();

            // Test that are changes are definately correct.
            user = await Database.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
                Assert.Fail("Logic error with the test, not worth continuing. Changes not implemented to the database");
            if(!user.MobileApplicationConfirmed || user.UserToken != userUserToken)
                Assert.Fail("Logic error with the test, not worth continuing. Changes not implemented to the database.");

            // Check that the controller reports correctly when in the correct scenario.
            if(!await controllerUnderTest.DeactivateUserToken(user.UserName, user.UserToken))
                Assert.Fail("Controller failed to return the correct result with the correct input.");

            // Get the User back out the database.
            user = await Database.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                Assert.Fail("Logic error with the test, not worth continuing. Changes not implemented to the database");

            if(user.MobileApplicationConfirmed)
                Assert.Fail("The controller did not update the User correctly.");

            if(user.UserToken != null)
                Assert.Fail("Controller has not cleared the User's User Token");
        }



        protected override void SeedDatabase()
        {
            base.SeedDatabase();
        }
    }
}
