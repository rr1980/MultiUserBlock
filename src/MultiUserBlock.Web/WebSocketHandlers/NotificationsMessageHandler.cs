using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MultiUserBlock.Common;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.WebSockets;

namespace MultiUserBlock.Web.WebSocketHandlers
{
    public class NotificationsMessageHandler : WebSocketHandler, INotificationsMessageHandler
    {
        private readonly UserRoleType urt = UserRoleType.Default;
        private readonly IRepository _repository;

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, IRepository Repository) : base(webSocketConnectionManager)
        {
            _repository = Repository;
        }

        public async void TestMethode(WebSocket socket, HttpContext httpContext, string name)
        {
            var id = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (!await _repository.HasRole(id, urt))
            {
                return;
            }

            await InvokeClientMethodAsync(socket, "receiveMessage", "JoJo");
        }
    }
}
