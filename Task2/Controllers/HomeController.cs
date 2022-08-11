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
        IWebHostEnvironment _appEnvironment;

        public HomeController(IFilesProcessor filesProcessor, ApplicationDbContext context, IWebHostEnvironment appEnvironment)
        {
            _filesProcessor = filesProcessor;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetLoadedFiles()
        {
            return Json(await _filesProcessor.GetLoadedFilesAsync(_context));
        }

        [HttpGet("load")]
        public IActionResult GetLoadingPage()
        {
            return View("LoadFile");
        }

        [HttpPost("load")]
        public async Task<IActionResult> LoadFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/files/" + uploadedFile.FileName;
                // сохраняем файл в папку files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                await _filesProcessor.LoadFileToDbAsync(_appEnvironment.WebRootPath + path, _context);
                return Redirect("/");
            }
            return BadRequest("No file attached!");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(int id)
        
        {
            return View("File", await _filesProcessor.GetFileAsTableAsync(id, _context));
        }
    }
}