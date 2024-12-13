using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckFuction.Interfaces
{
    public interface IEnvironmentService
    {
        string GetVariable(string key);
    }
}
