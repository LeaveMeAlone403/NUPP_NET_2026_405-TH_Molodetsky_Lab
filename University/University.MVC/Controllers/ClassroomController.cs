using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Infrastructure.Data;
using University.Infrastructure.Models;
using University.MVC.Models.ViewModels;

namespace University.MVC.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly UniversityContext _context;

        public ClassroomController(UniversityContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Classrooms
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    HasProjector = c.HasProjector,
                    TeacherId = c.TeacherId
                })
                .ToListAsync();
            return View(items);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var vm = await _context.Classrooms
                .Where(c => c.Id == id)
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    HasProjector = c.HasProjector,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new ClassroomModel
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = model.RoomNumber,
                    Capacity = model.Capacity,
                    HasProjector = model.HasProjector,
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
            var vm = await _context.Classrooms
                .Where(c => c.Id == id)
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    HasProjector = c.HasProjector,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClassroomViewModel model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                var entity = await _context.Classrooms.FindAsync(id);
                if (entity == null) return NotFound();

                entity.RoomNumber = model.RoomNumber;
                entity.Capacity = model.Capacity;
                entity.HasProjector = model.HasProjector;
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
            var vm = await _context.Classrooms
                .Where(c => c.Id == id)
                .Select(c => new ClassroomViewModel
                {
                    Id = c.Id,
                    RoomNumber = c.RoomNumber,
                    Capacity = c.Capacity,
                    HasProjector = c.HasProjector,
                    TeacherId = c.TeacherId
                })
                .FirstOrDefaultAsync();
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var entity = await _context.Classrooms.FindAsync(id);
            if (entity != null)
            {
                _context.Classrooms.Remove(entity);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}