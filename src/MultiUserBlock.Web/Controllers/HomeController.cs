using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiUserBlock.ViewModels;

namespace MultiUserBlock.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [Authorize(Policy = "ReadPolicy")]
        public IActionResult Index()
        {
            return View(new HomeViewModel());
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
