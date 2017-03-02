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
        private readonly IUserRepository _userRepository;

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, IUserRepository UserRepository) : base(webSocketConnectionManager)
        {
            _userRepository = UserRepository;
        }

        public async void TestMethode(WebSocket socket, HttpContext httpContext, string name)
        {
            var id = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (!await _userRepository.HasRole(id, urt))
            {
                return;
            }

            await InvokeClientMethodAsync(socket, "receiveMessage", "JoJo");
        }
    }
}
