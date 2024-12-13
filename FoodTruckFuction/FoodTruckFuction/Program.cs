using FoodTruckFuction.Interfaces;
using FoodTruckFuction.Respository;
using FoodTruckFuction.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
builder.Services.AddTransient<IFoodTruckRepository, FoodTruckRepository>();
builder.Services.AddTransient<IFoodTruckService, FoodTruckService>();

builder.ConfigureFunctionsWebApplication();
builder.Build().Run();
