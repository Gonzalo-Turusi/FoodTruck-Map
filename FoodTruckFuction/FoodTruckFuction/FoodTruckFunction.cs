using FoodTruckFuction.Interfaces;
using FoodTruckFuction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Claims;

namespace FoodTruckFunction
{
    public class FoodTruckFunction
    {
        private readonly ILogger _logger;
        private readonly IFoodTruckService _foodTruckService;
        private readonly IJwtHelper _jwtHelper;
        private readonly string _apiUrl;

        public FoodTruckFunction(ILoggerFactory loggerFactory, IJwtHelper jwtHelper, IFoodTruckService foodTruckService)
        {
            _logger = loggerFactory.CreateLogger<FoodTruckFunction>();
            _foodTruckService = foodTruckService;
            _jwtHelper = jwtHelper;
            // Retrieve the API URL from environment variables
            _apiUrl = Environment.GetEnvironmentVariable("API_URL");
            if (string.IsNullOrEmpty(_apiUrl))
            {
                throw new InvalidOperationException("API_URL environment variable is not set.");
            }
        }

        [Function("Hello")]
        public async Task<HttpResponseData> Hello(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello")] HttpRequestData req)
        {
            _logger.LogInformation("Processing request for Hello message.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync("¡Hi, if you find this function, you can follow me on https://github.com/Gonzalo-Turusi!");

            return response;
        }


        [Function("GetFoodTrucks")]
        public async Task<HttpResponseData> FoodTrucks(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "foodtrucks")] HttpRequestData req)
        {
            var token = req.Headers.FirstOrDefault(h => h.Key == "Authorization").Value.FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Authorization header missing");
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Missing Authorization Header");
                return unauthorizedResponse;
            }

            ClaimsPrincipal claims;
            try
            {
                claims = _jwtHelper.ValidateToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid token");
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Invalid Token");
                return unauthorizedResponse;
            }

            try
            {
                var userName = claims.FindFirst(ClaimTypes.Name)?.Value;
                _logger.LogInformation($"Authenticated user: {userName}");

                var foodTruckData = await _foodTruckService.GetFoodTrucks();

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(foodTruckData);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching food trucks");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal Server Error");
                return errorResponse;
            }
        }
    }
}