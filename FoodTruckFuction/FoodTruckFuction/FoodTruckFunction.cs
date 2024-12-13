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
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IFoodTruckService _foodTruckService;
        private readonly string _apiUrl;

        public FoodTruckFunction(ILoggerFactory loggerFactory, IFoodTruckService foodTruckService)
        {
            _httpClient = new HttpClient();

            _logger = loggerFactory.CreateLogger<FoodTruckFunction>();
            _foodTruckService = foodTruckService;
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
            return await _foodTruckService.GetFoodTrucks(req);
        }
    }
}