using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.MVC.Models.ViewModels;

namespace University.MVC.Controllers
{
    public class TeacherController : Controller
    {
        private readonly UniversityContext _context;

        public TeacherController(UniversityContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Teachers
                .Include(t => t.Courses)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Age = t.Age,
                    Salary = t.Salary,
                    AcademicExperienceYears = t.AcademicExperienceYears,
                    CourseIds = t.Courses.Select(c => c.Id).ToList()
                })
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Teachers
                .Where(t => t.Id == id)
                .Include(t => t.Courses)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Age = t.Age,
                    Salary = t.Salary,
                    AcademicExperienceYears = t.AcademicExperienceYears,
                    CourseIds = t.Courses.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new TeacherModel
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Age = model.Age,
                    Salary = model.Salary,
                    AcademicExperienceYears = model.AcademicExperienceYears
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
            var vm = await _context.Teachers
                .Where(t => t.Id == id)
                .Include(t => t.Courses)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Age = t.Age,
                    Salary = t.Salary,
                    AcademicExperienceYears = t.AcademicExperienceYears,
                    CourseIds = t.Courses.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TeacherViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var entity = await _context.Teachers.FindAsync(id);
                if (entity == null) return NotFound();

                entity.Name = model.Name;
                entity.Age = model.Age;
                entity.Salary = model.Salary;
                entity.AcademicExperienceYears = model.AcademicExperienceYears;

                _context.Update(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Teachers
                .Where(t => t.Id == id)
                .Include(t => t.Courses)
                .Select(t => new TeacherViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Age = t.Age,
                    Salary = t.Salary,
                    AcademicExperienceYears = t.AcademicExperienceYears,
                    CourseIds = t.Courses.Select(c => c.Id).ToList()
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Teachers.FindAsync(id);
            if (entity != null)
            {
                _context.Teachers.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}