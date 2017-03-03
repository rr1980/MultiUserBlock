using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public HomeController(IUserRepository UserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = UserRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new HomeViewModel()
            {
                CurrentUser = await _userRepository.GetById(id)
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
