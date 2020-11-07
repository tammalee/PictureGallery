using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public IActionResult Create(string title)
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

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if( files.Count <= 0)
            {
                ViewBag.Error = "You must select a file to upload.";
                return View("Create");
            }
            
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();

            foreach (var formFile in files)
            {
                if( formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/uploads", formFile.FileName);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //return View("Create");
            return Ok(new { count = files.Count, size, filePaths });
        }
    }
}
