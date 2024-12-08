using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class Security
{
    private readonly ILogger _logger;

    public Security(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Security>();
    }

    [Function("GenerateToken")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "token")] HttpRequestData req)
    {
        try
        {
            // Leer las credenciales del cuerpo de la solicitud (si aplica)
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var credentials = JsonSerializer.Deserialize<UserCredentials>(requestBody);

            if (credentials == null || string.IsNullOrEmpty(credentials.Username) || string.IsNullOrEmpty(credentials.Password))
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid credentials");
                return badRequestResponse;
            }

            // Validar credenciales (por simplicidad, usamos un check estático)
            if (credentials.Username == "admin" && credentials.Password == "password")
            {
                // Generar el token
                var token = JwtHelper.GenerateToken(credentials.Username, "Admin");

                // Devolver el token
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/json; charset=utf-8");
                await response.WriteStringAsync(JsonSerializer.Serialize(new { token }));
                return response;
            }

            var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
            await unauthorizedResponse.WriteStringAsync("Invalid username or password");
            return unauthorizedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating token: {ex.Message}");
            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("An error occurred while generating the token");
            return errorResponse;
        }
    }
}

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
}
