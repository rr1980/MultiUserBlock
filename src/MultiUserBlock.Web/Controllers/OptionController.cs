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
        private readonly IRepository _repository;
        private readonly HttpContext _httpContext;

        public OptionController(IRepository Repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = Repository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new OptionViewModel()
            {
                CurrentUser = await _repository.GetById(id),
                LayoutThemeViewModels = await _repository.GetAllThemes()
            });
        }

        [Authorize(Policy = "DefaultPolicy")]
        public async Task<bool> SaveUser(UserViewModel user)
        {

            await _repository.AddOrUpdate(user);


            return true;
        }

    }
}
