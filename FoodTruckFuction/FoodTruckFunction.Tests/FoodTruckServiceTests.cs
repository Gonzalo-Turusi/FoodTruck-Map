using FoodTruckFuction.Interfaces;
using FoodTruckFuction.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

public class FoodTruckServiceTests
{
    private readonly Mock<IFoodTruckRepository> _mockRepository;
    private readonly Mock<ILogger<FoodTruckService>> _mockLogger;
    private readonly FoodTruckService _service;

    public FoodTruckServiceTests()
    {
        _mockRepository = new Mock<IFoodTruckRepository>();
        _mockLogger = new Mock<ILogger<FoodTruckService>>();

        _service = new FoodTruckService(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetFoodTrucksForUser_ReturnsFoodTrucks()
    {
        // Arrange
        _mockRepository.Setup(r => r.FetchFoodTrucksAsync()).ReturnsAsync("[{\"name\":\"Truck 1\"}]");

        // Act
        var result = await _service.GetFoodTrucks();

        // Assert
        Assert.Equal("[{\"name\":\"Truck 1\"}]", result);
        _mockRepository.Verify(r => r.FetchFoodTrucksAsync(), Times.Once);
    }

}
