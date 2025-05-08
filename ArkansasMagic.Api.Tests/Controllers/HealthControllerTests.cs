using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArkansasMagic.Contracts;
using ArkansasMagic.Contracts.Health;
using ArkansasMagic.Domain.Http;
using System.Net;
using System.Threading.Tasks;
using static ArkansasMagic.Api.Tests.AssemblyInitialization;

namespace ArkansasMagic.Api.Tests.Controllers
{
    [TestClass]
    public class HealthControllerTests
    {
        [TestMethod]
        public async Task HealthCheckAsync_Returns200OK()
        {
            // Arrange & Act
            var (response, check) = await Client.GetAsJsonWithResponseAsync<CheckResponse>($"{ApiRoutes.v1.Health}/check");

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(check);
            Assert.IsTrue(check.ThumbsUp);
        }
    }
}
