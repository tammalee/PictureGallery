using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PictureGallery.Controllers
{
    public class PictureController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Create()
        {
            IActionResult action;
            if( User.Identity.IsAuthenticated )
            {
                action = View();
            }
            else
            {
                // if not logged in, redirect to gallery
                action = RedirectToAction("List");
            }
            return action;
        }
    }
}
