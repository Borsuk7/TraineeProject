using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TraineeProject.Models;
using TraineeProject.Models.DB;

namespace TraineeProject.Controllers
{
    public class HomeController : Controller
    {
        readonly ApplicationDbContext context;
        readonly IWebHostEnvironment appEnvironment;
        public HomeController(ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            this.context = context;
            this.appEnvironment = appEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile is not null)
            {
                var path = appEnvironment.WebRootPath + "/Files/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                PersonMap personMap = new();
                var people = personMap.ReadCSVFile(path);

                FileInfo fileInfo = new(path);
                fileInfo.Delete();

                if (people is null)
                {
                    return RedirectToAction("Index", "Home", new { error = "File is not valid" });
                }

                await context.People.AddRangeAsync(people);
                await context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index(string error = null)
        {
            ViewBag.error = error;
            return View();
        }
    }
}
