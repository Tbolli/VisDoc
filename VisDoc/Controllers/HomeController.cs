using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VisDoc.Models;
using System.Data.SqlClient;
using VisDoc.Data;

namespace VisDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VisDbContext _context;

        public HomeController(ILogger<HomeController> logger, VisDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index(string search)
        {
            if(string.IsNullOrWhiteSpace(search))
            {
                var model = _context.Document.ToList();
                return View(model);
            }

            var searchModel = from doc in _context.Document where doc.Name == search select doc;
            return View(searchModel);
            
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {

            foreach (var file in files)
            {
                string wwwPath = "C:\\Users\\Thomas Bolli\\Desktop\\dotnets\\VisDoc\\VisDoc\\xTemp";
                string pathString = Path.Combine(wwwPath, Path.GetFileName(file.FileName));

                using (var stream = new FileStream(pathString, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return Ok("ok");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}