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
    public class OptionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public OptionController(IUserRepository UserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = UserRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new OptionViewModel()
            {
                CurrentUser = await _userRepository.GetById(id),
                LayoutThemeViewModels = await _userRepository.GetAllThemes()
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<bool> SaveUser(UserViewModel user)
        {

            await _userRepository.AddOrUpdate(user);


            return true;
        }

    }
}
