using FoodTruckFuction.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckFuction.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
