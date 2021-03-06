﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public AccountController(IUserRepository UserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = UserRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //bool isAuth = await _accountService.Auth(model.Username, model.Password);
                if (await _auth(model.Username, model.Password))
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return Redirect("~/");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password wrong!");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("~/");
        }

        private async Task<bool> _auth(string username, string password)
        {
            var user = await _userRepository.GetByUserName(username);

            if (user == null || (user.Password != password))
            {
                return false;
            }

            var claims = new List<Claim> {
                                 new Claim(ClaimTypes.Authentication, "true"),
                                 new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                                 new Claim(ClaimTypes.Surname, user.Name),
                                 new Claim(ClaimTypes.GivenName, user.Vorname),
                                 new Claim(ClaimTypes.Name, user.Username)
                        };

            var uroles = user.Roles.Select(r => new Claim(ClaimTypes.Role, ((UserRoleType)r).ToString()));
            
            foreach (var role in uroles)
            {
                claims.Add(role);
            }


            var claimsIdentity = new ClaimsIdentity(claims, "password");
            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);


            await _httpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddHours(12),
                IsPersistent = true,            // Todo remember me!?
                AllowRefresh = true
            });

            return true;
        }
    }
}
