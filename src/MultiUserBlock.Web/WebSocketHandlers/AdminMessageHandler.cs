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
using MultiUserBlock.ViewModels;
using MultiUserBlock.WebSockets;
using Newtonsoft.Json;

namespace MultiUserBlock.Web.WebSocketHandlers
{
    public class AdminMessageHandler : WebSocketHandler, IAdminMessageHandler
    {
        private readonly UserRoleType urt = UserRoleType.Admin;
        private readonly IUserRepository _userRepository;


        public AdminMessageHandler(WebSocketConnectionManager webSocketConnectionManager, IUserRepository UserRepository) : base(webSocketConnectionManager)
        {
            _userRepository = UserRepository;
        }

        public async void SaveUser(WebSocket socket, HttpContext httpContext, dynamic user)
        {
            var id = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            if (!await _userRepository.HasRole(id, urt))
            {
                return;
            }
            var usr = JsonConvert.DeserializeObject<UserViewModel>(user.ToString());

            await InvokeClientMethodAsync(socket, "receiveMessage", "ADMIN");
        }
    }
}
