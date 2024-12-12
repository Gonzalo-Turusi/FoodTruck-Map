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
        private readonly string _apiUrl;

        public FoodTruckFunction(ILoggerFactory loggerFactory)
        {
            _httpClient = new HttpClient();

            _logger = loggerFactory.CreateLogger<FoodTruckFunction>();

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
            // Extraer el token del encabezado Authorization
            var token = req.Headers.FirstOrDefault(h => h.Key == "Authorization").Value.FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync("Missing Authorization Header");
                return unauthorizedResponse;
            }

            try
            {
                // Validar el token
                var claimsPrincipal = JwtHelper.ValidateToken(token);

                // (Opcional) Extraer información del usuario
                var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                var role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value;

                _logger.LogInformation($"Usuario autenticado: {username} con rol: {role}");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                await unauthorizedResponse.WriteStringAsync($"Token inválido: {ex.Message}");
                return unauthorizedResponse;
            }

            return await GetFoodTrucks(req);
        }

        private async Task<HttpResponseData> GetFoodTrucks(HttpRequestData req)
        {
            _logger.LogInformation("Processing request for food trucks.");

            try
            {
                var apiResponse = await _httpClient.GetAsync(_apiUrl);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error fetching data: {apiResponse.StatusCode}");
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }

                var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(jsonResponse);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error: {ex.Message}");
                return errorResponse;
            }
        }
    }
}