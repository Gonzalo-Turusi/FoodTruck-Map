using Microsoft.Azure.Functions.Worker.Http;

namespace FoodTruckFuction.Interfaces
{
    public interface IFoodTruckService
    {
        Task<string> GetFoodTrucks();
    }
}
