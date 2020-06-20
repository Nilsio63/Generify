using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Generify.Services.Interfaces.Management
{
    public interface IAuthenticationService
    {
        Task SaveAccessTokenAsync(string userId, string accessToken);
    }
}
