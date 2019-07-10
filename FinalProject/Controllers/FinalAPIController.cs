using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using FinalProject.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalAPIController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment_;
        private readonly ApplicationDbContext context_;
        private string webRootPath = null;
        private string filePath = null;

        public FinalAPIController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            hostingEnvironment_ = hostingEnvironment;
            webRootPath = hostingEnvironment_.WebRootPath;
            context_ = context;
            filePath = Path.Combine(webRootPath, "documents");
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var file = context_.documents.Find(id);
            var path = file.FilePath;
            if ((System.IO.File.Exists(path)))
            {
                System.IO.File.Delete(path);
                context_.documents.Remove(file);
                context_.SaveChanges();
            }
            
            return RedirectToAction("files", "Final");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            var request = HttpContext.Request;
            string name = request.Form["FileName"];
            string description = request.Form["FileDescription"];
            foreach (var file in request.Form.Files)
            {
                if (file.Length > 0)
                {
                    Files f = new Files();
                    var path = Path.Combine(filePath, file.FileName);
                    f.FilePath = path;
                    f.FileName = file.FileName;
                    f.FileDescription = description;
                    context_.documents.Add(f);
                    context_.SaveChanges();
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                    return BadRequest();
            }
            return RedirectToAction("files", "Final");
        }

        //[Roles("Admin", "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(int id, int? check)
        {
            if(check==null)
            {
                List<string> files = null;
                string file1 = "";
                try
                {
                    files = Directory.GetFiles(filePath).ToList<string>();
                    if (0 <= id && id < files.Count)
                        file1 = Path.GetFileName(files[id]);
                    else
                        return NotFound();
                }
                catch
                {
                    return NotFound();
                }
                file1 = files[id];
                var mem = new MemoryStream();
                using (var stream = new FileStream(file1, FileMode.Open))
                {
                    await stream.CopyToAsync(mem);
                }
                mem.Position = 0;
                return File(mem, GetContentType(file1), Path.GetFileName(file1));
            }
            var memory = new MemoryStream();
            var file = context_.documents.Find(id);
            var path = file.FilePath;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        public class RolesAttribute : AuthorizeAttribute
        {
            public RolesAttribute(params string[] roles)
            {
                Roles = String.Join(",", roles);
            }
        }

    }
}
