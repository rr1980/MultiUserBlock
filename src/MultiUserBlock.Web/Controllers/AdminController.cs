using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MultiUserBlock.Common.Repository;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public AdminController(IUserRepository UserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = UserRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var result = await _userRepository.GetAll();
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return View(new AdminViewModel()
            {
                Users = result,
                CurrentUser = await _userRepository.GetById(id)
            });
        }

        public async Task<AdminViewModel> SaveUser(UserViewModel user)
        {
            List<UserViewModel> result;

            if (!ModelState.IsValid)
            {
                result = await _userRepository.GetAll();
                result.Insert(0, new UserViewModel()
                {
                    UserId = -1,
                    ShowName = "Neu...",
                    Roles = new int[] { -1 }
                });

                return new AdminViewModel()
                {
                    Users = result,
                    Errors = GetModelStateErrors(ModelState)
                };
            }

            await _userRepository.AddOrUpdate(user);
            result = await _userRepository.GetAll();
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        public async Task ResetPassord(UserViewModel user)
        {
            await _userRepository.ResetPassword(user.UserId);
        }

        public async Task<AdminViewModel> DelUser(UserViewModel user)
        {
            await _userRepository.Remove(user.UserId);
            var result = await _userRepository.GetAll();
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return new AdminViewModel()
            {
                Users = result,
            };
        }

        public IActionResult Error()
        {
            return View();
        }


        public List<string> GetModelStateErrors(ModelStateDictionary ModelState)
        {
            List<string> errorMessages = new List<string>();

            var validationErrors = ModelState.Values.Select(x => x.Errors);
            validationErrors.ToList().ForEach(ve =>
            {
                var errorStrings = ve.Select(x => x.ErrorMessage);
                errorStrings.ToList().ForEach(em =>
                {
                    errorMessages.Add(em);
                });
            });

            return errorMessages;
        }
    }
}



//_logger.LogWarning("loulou");
//_logger.LogError("loulou");
//_logger.LogWarning(LoggingEvents.GET_ITEM, "Getting item {ID}", 1);