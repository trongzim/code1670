using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FPTJOB.Models;

namespace FPTJOB.Controllers
{
    public class JobsController : Controller
    {
        private readonly DBMyContext _context;

        public JobsController(DBMyContext context)
        {
            _context = context;
        }

        // GET: Jobs
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Lấy danh sách công việc
            var jobs = _context.Jobs.Include(j => j.Category).AsQueryable();

            // Sắp xếp danh sách công việc theo lương
            switch (sortOrder)
            {
                case "salary_asc":
                    jobs = jobs.OrderBy(j => j.Salary); // Lương tăng dần
                    break;
                case "salary_desc":
                    jobs = jobs.OrderByDescending(j => j.Salary); // Lương giảm dần
                    break;
                default:
                    break;
            }

            // Đếm tổng số công việc
            ViewBag.TotalJobs = await _context.Jobs.CountAsync();
            ViewBag.SortOrder = sortOrder;

            return View(await jobs.ToListAsync());
        }

        // GET: Jobs/Create
        [Authorize(Roles = "Admin, Employer")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories, "Id", "Name");
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Industry,Location,Description,Requirement,Deadline,Salary,CategoryId")] Job job)
        {
            if (job.Salary <= 0)
            {
                ModelState.AddModelError("Salary", "Salary must be greater than 0.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories, "Id", "Name", job.CategoryId);
            return View(job);
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories, "Id", "Name", job.CategoryId);
            return View(job);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Industry,Location,Description,Requirement,Deadline,Salary,CategoryId")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (job.Salary <= 0)
            {
                ModelState.AddModelError("Salary", "Salary must be greater than 0.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
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

            ViewData["CategoryId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Categories, "Id", "Name", job.CategoryId);
            return View(job);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return (_context.Jobs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
