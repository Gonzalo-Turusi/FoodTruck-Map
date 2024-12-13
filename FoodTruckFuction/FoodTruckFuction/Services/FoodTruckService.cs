using FoodTruckFuction.Interfaces;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckFuction.Services
{
    public class FoodTruckService : IFoodTruckService
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IFoodTruckRepository _repository;
        private readonly ILogger<FoodTruckService> _logger;

        public FoodTruckService(IJwtHelper jwtHelper, IFoodTruckRepository repository, ILogger<FoodTruckService> logger)
        {
            _jwtHelper = jwtHelper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<HttpResponseData> GetFoodTrucks(HttpRequestData req)
        {
            try
            {
                var token = req.Headers.FirstOrDefault(h => h.Key == "Authorization").Value.FirstOrDefault()?.Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return req.CreateResponse(HttpStatusCode.Unauthorized);
                }

                var claims = _jwtHelper.ValidateToken(token);
                _logger.LogInformation($"Authenticated user: {claims.FindFirst(ClaimTypes.Name)?.Value}");

                var foodTruckData = await _repository.FetchFoodTrucksAsync();
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(foodTruckData);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing request");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"Error: {ex.Message}");
                return response;
            }
        }
    }
}
