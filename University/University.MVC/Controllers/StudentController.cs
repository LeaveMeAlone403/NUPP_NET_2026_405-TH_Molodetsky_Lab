using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.MVC.Models.ViewModels;

namespace University.MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly UniversityContext _context;

        public StudentController(UniversityContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Students
                .Include(s => s.Enrollments)
                .Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Group = s.Group,
                    LastGrade = s.LastGrade,
                    AverageScore = s.AverageScore,
                    EnrollmentIds = s.Enrollments.Select(e => e.Id).ToList()
                })
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Students
                .Where(s => s.Id == id)
                .Include(s => s.Enrollments)
                .Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Group = s.Group,
                    LastGrade = s.LastGrade,
                    AverageScore = s.AverageScore,
                    EnrollmentIds = s.Enrollments.Select(e => e.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new StudentModel
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Age = model.Age,
                    Group = model.Group,
                    LastGrade = model.LastGrade,
                    AverageScore = model.AverageScore
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
            var vm = await _context.Students
                .Where(s => s.Id == id)
                .Include(s => s.Enrollments)
                .Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Group = s.Group,
                    LastGrade = s.LastGrade,
                    AverageScore = s.AverageScore,
                    EnrollmentIds = s.Enrollments.Select(e => e.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var entity = await _context.Students.FindAsync(id);
                if (entity == null) return NotFound();

                entity.Name = model.Name;
                entity.Age = model.Age;
                entity.Group = model.Group;
                entity.LastGrade = model.LastGrade;
                entity.AverageScore = model.AverageScore;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Students
                .Where(s => s.Id == id)
                .Include(s => s.Enrollments)
                .Select(s => new StudentViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Age = s.Age,
                    Group = s.Group,
                    LastGrade = s.LastGrade,
                    AverageScore = s.AverageScore,
                    EnrollmentIds = s.Enrollments.Select(e => e.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Students.FindAsync(id);
            if (entity != null)
            {
                _context.Students.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id) => _context.Students.Any(e => e.Id == id);
    }
}