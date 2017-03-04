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
        private readonly IRepository _repository;
        private readonly HttpContext _httpContext;

        public AdminController(IRepository Repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = Repository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index()
        {
            var id = Convert.ToInt32(_httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var result = await _repository.GetAll();
            result.Insert(0, new UserViewModel()
            {
                UserId = -1,
                ShowName = "Neu...",
                Roles = new int[] { -1 }
            });

            return View(new AdminViewModel()
            {
                Users = result,
                CurrentUser = await _repository.GetById(id)
            });
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<AdminViewModel> SaveUser(UserViewModel user)
        {
            List<UserViewModel> result;

            if (!ModelState.IsValid)
            {
                result = await _repository.GetAll();
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

            await _repository.AddOrUpdate(user);
            result = await _repository.GetAll();
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

        [Authorize(Policy = "AdminPolicy")]
        public async Task ResetPassord(UserViewModel user)
        {
            await _repository.ResetPassword(user.UserId);
        }

        [Authorize(Policy = "AdminPolicy")]
        public async Task<AdminViewModel> DelUser(UserViewModel user)
        {
            await _repository.Remove(user.UserId);
            var result = await _repository.GetAll();
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

        [Authorize]
        public IActionResult Error()
        {
            return View();
        }


        private List<string> GetModelStateErrors(ModelStateDictionary ModelState)
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