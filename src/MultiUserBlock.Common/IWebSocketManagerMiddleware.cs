using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MultiUserBlock.Common
{
    public interface IWebSocketManagerMiddleware
    {
        Task Invoke(HttpContext context);
    }
}
