using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace FPTJOB.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly DBMyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfilesController(DBMyContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin, Seeker")]
        public async Task<IActionResult> Index()
        {
            if (_context.Profiles == null)
                return Problem("Entity set 'DBMyContext.Profiles' is null.");

            return View(await _context.Profiles.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profiles == null)
                return NotFound();

            var profile = await _context.Profiles.FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        [Authorize(Roles = "Admin, Seeker")]
        public IActionResult Create()
        {
            ViewBag.UserID = User.Identity.Name;
            return View();
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,FullName,Address,City,Skill,Education,MyFile,UploadFile")] Profile profile)
        {
            ModelState.Remove("MyFile");

            if (!ModelState.IsValid)
                return View(profile);

            if (profile.UploadFile != null)
            {
                string uniqueFileName = GetUniqueFileName(profile.UploadFile.FileName);
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Ensure directory exists

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profile.UploadFile.CopyToAsync(fileStream);
                }

                profile.MyFile = uniqueFileName;
            }

            _context.Add(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Seeker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profiles == null)
                return NotFound();

            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,FullName,Address,City,Skill,Education,MyFile")] Profile profile)
        {
            if (id != profile.Id)
                return NotFound();

            ModelState.Remove("UploadFile");

            if (!ModelState.IsValid)
                return View(profile);

            try
            {
                _context.Update(profile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(profile.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Seeker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profiles == null)
                return NotFound();

            var profile = await _context.Profiles.FirstOrDefaultAsync(m => m.Id == id);

            if (profile == null)
                return NotFound();

            return View(profile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profiles == null)
                return Problem("Entity set 'DBMyContext.Profiles' is null.");

            var profile = await _context.Profiles.FindAsync(id);

            if (profile != null)
                _context.Profiles.Remove(profile);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileExists(int id)
        {
            return (_context.Profiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
