using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MultiUserBlock.Common
{
    public interface INotificationsMessageHandler
    {
        void TestMethode(WebSocket socket, HttpContext httpContext, string name);
    }
}
