using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.MVC.Models.ViewModels;

namespace University.MVC.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly UniversityContext _context;

        public EnrollmentController(UniversityContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Enrollments
                .Select(e => new EnrollmentViewModel
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Grade = e.Grade
                })
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Enrollments
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentViewModel
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Grade = e.Grade
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new EnrollmentModel
                {
                    Id = Guid.NewGuid(),
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    Grade = model.Grade
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
            var vm = await _context.Enrollments
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentViewModel
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Grade = e.Grade
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EnrollmentViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var entity = await _context.Enrollments.FindAsync(id);
                if (entity == null) return NotFound();

                entity.StudentId = model.StudentId;
                entity.CourseId = model.CourseId;
                entity.Grade = model.Grade;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Enrollments
                .Where(e => e.Id == id)
                .Select(e => new EnrollmentViewModel
                {
                    Id = e.Id,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    Grade = e.Grade
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Enrollments.FindAsync(id);
            if (entity != null)
            {
                _context.Enrollments.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}