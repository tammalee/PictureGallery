using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PictureGallery.Data;
using PictureGallery.Models;

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
            DirectoryInfo uploadsDirectory = null;
            FileInfo[] files = null;

            try
            {
                string uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                uploadsDirectory = new DirectoryInfo(uploadsPath);
                files = uploadsDirectory.GetFiles();
                ViewBag.Files = files;
            }
            catch (DirectoryNotFoundException e)
            {
                throw new Exception("Directory not found.");
            }
            catch (IOException e)
            {
                throw new Exception("Failed to access directory.");
            }
            

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
        public async Task<IActionResult> Upload( string title, List<IFormFile> files)
        {

            title = title != null ? title.Trim() : null;
            bool validTitle = validateTitle(title);

            if(!validTitle)
            {
                ViewBag.Error = "You must include a title and it can be no more than 50 characters";
                ViewBag.Title = title;
                return View("Create");
            }

            if ( files.Count <= 0)
            {
                ViewBag.Error = "You must select a file to upload.";
                ViewBag.Title = title;
                return View("Create");
            }
            
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            var pictureIDs = new List<int>();

            foreach (var formFile in files)
            {
                if( formFile.Length > 0)
                {
                    //For security reasons we want to rename the file
                    var oldFileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                    var fileExtension = Path.GetExtension(formFile.FileName);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    //upload the file to the uploads folder
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/uploads", newFileName);
                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            //return View("Create");
            return Ok(new { count = files.Count, size, filePaths, pictureIDs });
        }

        public bool validateTitle(string title)
        {
            var valid = true;

            if (string.IsNullOrWhiteSpace(title))
            {
                valid = false;
            }
            else
            {
                if (title.Length > 50)
                {
                    valid = false;
                }
            }

            return valid;
        }
    }
}
