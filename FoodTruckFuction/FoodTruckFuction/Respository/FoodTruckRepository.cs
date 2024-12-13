using FoodTruckFuction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckFuction.Respository
{
    public class FoodTruckRepository : IFoodTruckRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public FoodTruckRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiUrl = Environment.GetEnvironmentVariable("API_URL") ?? throw new InvalidOperationException("API_URL not set.");
        }

        public async Task<string> FetchFoodTrucksAsync()
        {
            var response = await _httpClient.GetAsync(_apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch data: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
