using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularSPA.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace AngularSPA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileLinksController : ControllerBase
    {
        private readonly FileDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public FileLinksController(FileDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: api/FileLinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileLink>>> GetLinks()
        {
            var files = await _context.Links.OrderBy(x => x.Date).ToListAsync();
            var filesInfo = new Dictionary<string, List<FileLink>>();
            var currentYear = DateTime.Now.Year;
            var dateFormat = "dd MMMM";
            var dateFormatWithYear = "dd MMMM yyyy";

            foreach (var file in files)
            {
                var fileKey = default(string);
                if (file.Date.Year == currentYear)
                {
                    fileKey = file.Date.ToString(dateFormat);
                }
                else
                {
                    fileKey = file.Date.ToString(dateFormatWithYear);
                }
                var dayFiles = new List<FileLink>();
                if (!filesInfo.ContainsKey(fileKey))
                {
                    filesInfo.Add(fileKey, new List<FileLink>());
                }
                filesInfo[fileKey].Add(file);
            }

            return Ok(filesInfo);
        }

        // GET: api/FileLinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileLink>> GetFileLink(int id)
        {
            var fileLink = await _context.Links.FindAsync(id);

            if (fileLink == null)
            {
                return NotFound();
            }

            var fileInfo = await _context.Files.FindAsync(fileLink.HashId);
            if (fileInfo == null)
            {
                return NotFound();
            }

            var net = new WebClient();
            var data = net.DownloadData(fileInfo.Location);
            var content = new MemoryStream(data);
            var contentType = "APPLICATION/force-download";
            var fileName = fileLink.Name;


            return File(content, contentType, fileName);
        }

        // PUT: api/FileLinks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileLink(int id, FileLink fileLink)
        {
            if (id != fileLink.Id)
            {
                return BadRequest();
            }

            _context.Entry(fileLink).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FileLinkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FileLinks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<FileLink>> PostFileLink()
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                var folderName = string.Format("Upload/{0}", DateTime.Now.ToString("yyyyMMdd"));
                var webRootPath = hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                using var hashArray = SHA256.Create();
                var bytes = hashArray.ComputeHash(file.OpenReadStream());

                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                var fileHash = builder.ToString();
                var hashId = 0;
                var checkFile = _context.Files.Where(f => f.Hash == fileHash).FirstOrDefault();
                var fileName = fileHash + '.' + file.FileName.Split('.')[^1];
                var fullPath = Path.Combine(newPath, fileName);


                if (checkFile == null)
                {
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(stream);


                    var fileInfo = new Models.FileInfo(fileHash, fullPath, file.Length.ToString());
                    _context.Files.Add(fileInfo);
                    await _context.SaveChangesAsync();
                    hashId = fileInfo.Id;
                }
                else
                {
                    hashId = checkFile.Id;
                }

                var fileLink = new FileLink(file.FileName, hashId, User.Identity.Name, DateTime.Now);

                var checkFileLink = _context.Links
                    .Where(l => l.Name == fileLink.Name
                    && l.Username == fileLink.Username
                    && l.HashId == fileLink.HashId).FirstOrDefault();

                if (checkFileLink == null)
                {
                    _context.Links.Add(fileLink);
                }
                else
                {
                    checkFileLink.Date = fileLink.Date;
                    _context.Update(checkFileLink);
                }

                await _context.SaveChangesAsync();
            }

            //return CreatedAtAction("GetFileLink", new { id = fileLink.Id }, fileLink);
            return Ok();
        }

        // DELETE: api/FileLinks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FileLink>> DeleteFileLink(int id)
        {
            var fileLink = await _context.Links.FindAsync(id);
            if (fileLink == null)
            {
                return NotFound();
            }

            _context.Links.Remove(fileLink);
            await _context.SaveChangesAsync();


            var fileLinks = _context.Links.Where(l => l.HashId == fileLink.HashId);
            if (fileLinks.Count() == 0)
            {
                var fileInfo = await _context.Files.FindAsync(fileLink.HashId);
                if (fileInfo != null)
                {
                    System.IO.File.Delete(fileInfo.Location);
                    _context.Files.Remove(fileInfo);
                }
            }
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool FileLinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
