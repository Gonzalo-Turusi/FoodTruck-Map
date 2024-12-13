using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Collections.Generic;
using FoodTruckFunction;
using Microsoft.Azure.Functions.Worker;
using FoodTruckFuction;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FoodtruckFunction.Tests;

namespace FoodTruckFunction.Tests.Functions
{
    [TestClass]
    public class FoodTruckFunctionTests
    {
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<IJwtHelper> _JwtHelperMock;
        private Mock<ILogger<FoodTruckFunction>> _loggerMock;
        private FoodTruckFunction _function;

        [TestInitialize]
        public void Setup()
        {
            // Configurar la variable de entorno API_URL
            Environment.SetEnvironmentVariable("API_URL", "https://example.com/api/foodtrucks");

            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerMock = new Mock<ILogger<FoodTruckFunction>>();
            _loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);
            _JwtHelperMock = new Mock<IJwtHelper>();
            _function = new FoodTruckFunction(_loggerFactoryMock.Object, _JwtHelperMock.Object);
        }

        [TestMethod]
        public async Task FoodTrucks_ReturnsUnauthorized_WhenTokenIsMissing()
        {
            var context = new Mock<FunctionContext>();
            var request = new Mock<HttpRequestData>(context.Object);

            var result = await _function.FoodTrucks(request.Object);

            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
            // Add more assertions to check the response content
        }

        [TestMethod]
        public async Task FoodTrucks_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            var context = new Mock<FunctionContext>();
            var request = new Mock<HttpRequestData>(context.Object);
            var headers = new HttpHeadersCollection();
            headers.Add("Authorization", "Bearer invalid_token");
            request.Setup(r => r.Headers).Returns(headers);
            var responseMock = new Mock<HttpResponseData>(context.Object);
            responseMock.SetupProperty(r => r.StatusCode, HttpStatusCode.Unauthorized);
            request.Setup(r => r.CreateResponse(It.IsAny<HttpStatusCode>())).Returns(responseMock.Object);

            var jwtHelperMock = new Mock<IJwtHelper>();
            jwtHelperMock.Setup(j => j.ValidateToken(It.IsAny<string>())).Throws(new SecurityTokenException("Invalid token"));

            var function = new FoodTruckFunction(_loggerFactoryMock.Object, jwtHelperMock.Object);

            var response = await function.FoodTrucks(request.Object);

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
            // Add more assertions to check the response content
        }

        [TestMethod]
        public async Task FoodTrucks_ReturnsOk_WhenTokenIsValid()
        {
            var context = new Mock<FunctionContext>();
            var request = new Mock<HttpRequestData>(context.Object);
            var headers = new HttpHeadersCollection();
            headers.Add("Authorization", "Bearer valid_token");
            request.Setup(r => r.Headers).Returns(headers);
            var responseMock = new Mock<HttpResponseData>(context.Object);
            responseMock.SetupProperty(r => r.StatusCode, HttpStatusCode.OK);
            request.Setup(r => r.CreateResponse(It.IsAny<HttpStatusCode>())).Returns(responseMock.Object);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            }));

            var jwtHelperMock = new Mock<IJwtHelper>();
            jwtHelperMock.Setup(j => j.ValidateToken(It.IsAny<string>())).Returns(claimsPrincipal);

            var function = new FoodTruckFunction(_loggerFactoryMock.Object, jwtHelperMock.Object);

            var response = await function.FoodTrucks(request.Object);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
