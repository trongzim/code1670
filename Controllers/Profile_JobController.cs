using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;

namespace FPTJOB.Controllers
{
    public class Profile_JobController : Controller
    {
        private readonly DBMyContext _context;

        public Profile_JobController(DBMyContext context)
        {
            _context = context;
        }

        // GET: Profile_Job
        public async Task<IActionResult> Index(int id)
        {
            var dBMyContext = _context.Profile_Job.Include(p => p.Job).Include(p => p.Profile).Include(p => p.Profile).Where(p => p.JobId == id);
            return View(await dBMyContext.ToListAsync());
        }

        // GET: Profile_Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profile_Job == null)
            {
                return NotFound();
            }

            var profile_Job = await _context.Profile_Job
                .Include(p => p.Job)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile_Job == null)
            {
                return NotFound();
            }

            return View(profile_Job);
        }

        // GET: Profile_Job/Create
        public IActionResult Create(int id)
        {
            Profile_Job pj = new Profile_Job();
            pj.JobId = id;
            pj.RegDate = DateTime.Now;
            pj.ProfileId = _context.Profiles.Where(p => p.UserID == User.Identity.Name).FirstOrDefault().Id;
            _context.Add(pj);
            _context.SaveChanges();

            return RedirectToAction("ListJob", "Jobs");
        }

        // POST: Profile_Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegDate,JobId,ProfileId")] Profile_Job profile_Job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile_Job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", profile_Job.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profile_Job.ProfileId);
            return View(profile_Job);
        }

        // GET: Profile_Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profile_Job == null)
            {
                return NotFound();
            }

            var profile_Job = await _context.Profile_Job.FindAsync(id);
            if (profile_Job == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", profile_Job.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profile_Job.ProfileId);
            return View(profile_Job);
        }

        // POST: Profile_Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegDate,JobId,ProfileId")] Profile_Job profile_Job)
        {
            if (id != profile_Job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profile_Job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Profile_JobExists(profile_Job.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Id", profile_Job.JobId);
            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profile_Job.ProfileId);
            return View(profile_Job);
        }

        // GET: Profile_Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profile_Job == null)
            {
                return NotFound();
            }

            var profile_Job = await _context.Profile_Job
                .Include(p => p.Job)
                .Include(p => p.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profile_Job == null)
            {
                return NotFound();
            }

            return View(profile_Job);
        }

        // POST: Profile_Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profile_Job == null)
            {
                return Problem("Entity set 'DBMyContext.Profile_Job'  is null.");
            }
            var profile_Job = await _context.Profile_Job.FindAsync(id);
            if (profile_Job != null)
            {
                _context.Profile_Job.Remove(profile_Job);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Profile_JobExists(int id)
        {
          return (_context.Profile_Job?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
