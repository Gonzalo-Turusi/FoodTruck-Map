namespace FoodTruckFuction.Interfaces
{
    public interface IFoodTruckRepository
    {
        Task<string> FetchFoodTrucksAsync();
    }
}
