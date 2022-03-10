using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VisDoc.Models;
using System.Data.SqlClient;
using VisDoc.Data;

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
        public IActionResult Index(string Search, string SearchParentSelect)
        {
            List<DocumentModel> model;
            if (string.IsNullOrWhiteSpace(Search))
            {
                model = _context.Document.ToList();
                return View(model);
            }
            model = _context.Document.Where(x => x.Name.Contains(Search)).OrderBy(x => x.Id).ToList();  
            return View(model);
        }

        public IActionResult ParentSelectViewPartial()
        {
            List<DocumentModel> model;
            model = _context.Document.ToList();
            return NoContent();

        }

        //Upload File
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Upload(List<IFormFile> files, string givenName)
        {
           if (files != null)
            {
                foreach (var file in files)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    if(fileExtension == ".zip") {  return Ok("Canot allow zipFiles"); }

                    string appeardFileName = string.IsNullOrWhiteSpace(givenName) ? file.FileName : givenName;
                    string uniqueFileName = Guid.NewGuid().ToString("N");
                    string pathString = Path.Combine(basePath, uniqueFileName +fileExtension);
                    using (var stream = new FileStream(pathString, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    DocumentModel newInsert = new DocumentModel
                    {
                        Name = appeardFileName,
                        Path = pathString,
                        UploadedDateTime = DateTime.Now,
                        Extension = fileExtension

                    };
                    _context.Document.Add(newInsert);
                    _context.SaveChanges();
                }
            }
            List<DocumentModel> model = _context.Document.ToList();
            return View("Index", model);
        }

        //UpdateInfo
        [HttpPost]
        public IActionResult UpdateInfo(string NameChange, string NameId)
        {
            if (string.IsNullOrWhiteSpace(NameChange)) { return RedirectToAction("Index", "Home"); }
            DocumentModel toBeChangedName = _context.Document.Where(doc => doc.Id == Int32.Parse(NameId)).FirstOrDefault();
            if(toBeChangedName != null)
            {
                toBeChangedName.Name = NameChange;
                _context.Entry(toBeChangedName).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
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