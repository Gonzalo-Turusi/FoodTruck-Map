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
        private readonly IFoodTruckRepository _repository;
        private readonly ILogger<FoodTruckService> _logger;

        public FoodTruckService(IFoodTruckRepository repository, ILogger<FoodTruckService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<string> GetFoodTrucks()
        {
            return await _repository.FetchFoodTrucksAsync();
        }
    }
}
