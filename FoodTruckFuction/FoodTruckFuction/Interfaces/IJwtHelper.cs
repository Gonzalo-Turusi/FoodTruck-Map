using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckFuction.Interfaces
{
    public interface IJwtHelper
    {
        string GenerateToken(string username, string role, int expireMinutes = 30);
        ClaimsPrincipal ValidateToken(string token);
    }

}
