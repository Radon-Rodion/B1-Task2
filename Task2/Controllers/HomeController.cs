using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task2.Data;
using Task2.Services;
using Microsoft.AspNetCore.Hosting;

namespace Task2.Controllers
{
    [Route("/api/files")]
    public class HomeController : Controller
    {
        private IFilesProcessor _filesProcessor;
        private ApplicationDbContext _context;
        IWebHostEnvironment _appEnvironment; //for temp saving loaded files on server

        public HomeController(IFilesProcessor filesProcessor, ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _filesProcessor = filesProcessor;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// Returns array of loaded files infos (id, path, name, balance periods)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLoadedFiles()
        {
            return Json(await _filesProcessor.GetLoadedFilesAsync(_context));
        }

        /// <summary>
        /// Returns RazorPage for choosing and loading excel file
        /// </summary>
        [HttpGet("load")]
        public IActionResult GetLoadingPage()
        {
            return View("LoadFile");
        }

        /// <summary>
        /// Loads file, saves info from it into database. 
        /// Redirects back in case of success
        /// </summary>
        [HttpPost("load")]
        public async Task<IActionResult> LoadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // saving to directory 'Files' in directory wwwroot
                string path = "/files/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                await _filesProcessor.LoadFileToDbAsync(_appEnvironment.WebRootPath + path, _context);
                return Redirect("/");
            }
            return BadRequest("No file attached!");
        }

        /// <summary>
        /// Returns RazorPage with table representing info from file in database
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(int id)
        
        {
            return View("File", await _filesProcessor.GetFileAsTableAsync(id, _context));
        }
    }
}