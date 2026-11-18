using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.MVC.Models.ViewModels;

namespace University.MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly UniversityContext _context;

        public CourseController(UniversityContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Courses
                .Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits,
                    TeacherId = c.TeacherId
                })
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new CourseModel
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Credits = model.Credits,
                    TeacherId = model.TeacherId
                };
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CourseViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var entity = await _context.Courses.FindAsync(id);
                if (entity == null) return NotFound();

                entity.Title = model.Title;
                entity.Credits = model.Credits;
                entity.TeacherId = model.TeacherId;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Credits = c.Credits,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Courses.FindAsync(id);
            if (entity != null)
            {
                _context.Courses.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}