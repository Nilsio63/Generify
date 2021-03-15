﻿using Generify.Models.Management;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace Generify.Blazor.Services
{
    public interface IGenerifyAuthenticationStateProvider
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task SetAuthenticatedAsync(User user);
        Task RemoveAuthenticatedAsync();
    }
}
