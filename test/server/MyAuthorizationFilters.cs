using Microsoft.AspNetCore.Http;
using Naruto.WebSocket.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server
{
    public class MyAuthorizationFilters : INarutoWeSocketAuthorizationFilters
    {
        public Task<bool> AuthorizationAsync(HttpContext context)
        {
            return Task.FromResult(true);
        }
    }
}
