using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VisDoc.Models;
using System.Data.SqlClient;
using VisDoc.Data;
using VisDoc.ControllerHelperFunctions;

namespace VisDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly VisDbContext _context;
        string basePath = Directory.GetCurrentDirectory() + @"\xTemp";
        public HomeController(ILogger<HomeController> logger, VisDbContext context)
        {
            _context = context;
        }
        //Get Index
        public IActionResult Index(string Search)
        {
            List<DocumentModel> model;
            if (string.IsNullOrWhiteSpace(Search))
            {
                model = _context.Document.ToList();
                return View(model);
            }
            model = _context.Document.Where(x => x.Name.Contains(Search)).ToList();  
            return View(model);
        }

        //Upload File
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var nameLikeList = _context.Document.Where(x => x.Name.Contains(file.FileName)).ToList();

                    

                    string uniqueFileName = Helper.newName(nameLikeList, file);

                    string pathString = Path.Combine(basePath, uniqueFileName);
                    using (var stream = new FileStream(pathString, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    DocumentModel newInsert = new DocumentModel
                    {
                        Name = uniqueFileName,
                        Path = pathString,
                        UploadedDateTime = DateTime.Now
                    };
                    _context.Document.Add(newInsert);
                    _context.SaveChanges();
                }
            }
            List<DocumentModel> model = _context.Document.ToList();
            //return Ok(model);
            return View("Index", model);
        }

        //Download File
        public  async Task<IActionResult> Download(string filename)
        {
            var memory = new MemoryStream();
            string path = Path.Combine(basePath, filename);

            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", Path.GetFileName(path));
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