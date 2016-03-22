using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicketManagement.Controllers.API;

namespace TicketManagement.Tests.Controllers.API
{
    [TestClass]
    public class BaseApiControllerTests : TestBase
    {
        [TestMethod]
        public void Test_BaseApiController_GetDefault()
        {
            // Setup/
            BaseApiController controllerUnderTest = new BaseApiController(Database);

            // Test the Controller method.
            string response = controllerUnderTest.Get();

            // Is the output as we expected.
            if (string.IsNullOrEmpty(response))
                Assert.Fail("No response from Controller.");
        }

        [TestMethod]
        public void Test_BaseApiController_GetWithId()
        {
            // Setup.
            int idParameter = 7;
            BaseApiController controllerUnderTest = new BaseApiController(Database);

            // Test the Controller method.
            string response = controllerUnderTest.Get(idParameter);

            // Does the output contain what we expected, roughly as text-text doesn't really matter, just that it incorperate the Id back.
            if (!response.Contains(idParameter.ToString()))
                Assert.Fail("Controller failed to incorperate the parameter in the response.");
        }

        [TestMethod]
        public void Test_BaseApiController_CheckConnection()
        {
            // Setup.
            BaseApiController controllerUnderTest = new BaseApiController(Database);

            // Test the Controller method. Did it resond as we expect.
            if (!controllerUnderTest.CheckConnection())
                Assert.Fail("Controller failed to respond correctly.");
        }

        //[TestMethod]
        //public async Task Test_BaseApiController_IsUserInternal()
        //{
        //    // Setup.
        //    SeedDatabase();
        //    BaseApiController controllerUnderTest = new BaseApiController(Database);

        //    // Check that we are set up as expected for the full test.
        //    List<User> users = Database.Users.ToList();
        //    if(users.Count != 2)
        //        Assert.Fail("Seeded database not as expected for this test.");

        //    // Check that we have the User we need for testing.
        //    User internalUser = await Database.Users.FirstOrDefaultAsync(u => u.FirstName == "Internal");
        //    if(internalUser == null)
        //        Assert.Fail("Could not get the seeded User rom the database.");

        //    bool result = controllerUnderTest.Is
        //}
    }
}
