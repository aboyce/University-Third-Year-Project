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

        protected override void Seed()
        {
            base.Seed();
        }
    }
}
