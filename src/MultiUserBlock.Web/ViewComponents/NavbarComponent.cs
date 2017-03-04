﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Web.ViewComponents
{
    public class NavbarComponent : ViewComponent
    {
        private readonly IRepository _repository;
        private readonly HttpContext _httpContext;

        public NavbarComponent(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            return View(new NavbarViewModel()
            {
                UserViewModel = await _repository.GetById(id)
            });
        }
    }
}
