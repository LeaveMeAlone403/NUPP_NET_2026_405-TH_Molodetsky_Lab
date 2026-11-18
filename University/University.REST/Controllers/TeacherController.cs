using Microsoft.AspNetCore.Mvc;
using University.Common;
using DBTeacher = University.Infrastructure.Models.TeacherModel;
using RESTTeacher = University.REST.Models.TeacherDto;

namespace University.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ICrudServiceAsync<DBTeacher> _service;

        public TeacherController(ICrudServiceAsync<DBTeacher> service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RESTTeacher>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items.Select(t => new RESTTeacher
            {
                Id = t.Id,
                Name = t.Name,
                Age = t.Age,
                Salary = t.Salary,
                AcademicExperienceYears = t.AcademicExperienceYears,
                ClassroomId = t.Classroom?.Id,
                CourseIds = t.Courses?.Select(c => c.Id).ToList() ?? new List<Guid>()
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RESTTeacher>> GetById(Guid id)
        {
            var t = await _service.ReadAsync(id);
            if (t == null) return NotFound();
            return Ok(new RESTTeacher
            {
                Id = t.Id,
                Name = t.Name,
                Age = t.Age,
                Salary = t.Salary,
                AcademicExperienceYears = t.AcademicExperienceYears,
                ClassroomId = t.Classroom?.Id,
                CourseIds = t.Courses?.Select(c => c.Id).ToList() ?? new List<Guid>()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RESTTeacher dto)
        {
            var entity = new DBTeacher
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Name = dto.Name,
                Age = dto.Age,
                Salary = dto.Salary,
                AcademicExperienceYears = dto.AcademicExperienceYears
            };

            if (!await _service.CreateAsync(entity)) return BadRequest();
            await _service.SaveAsync();

            // Оновлюємо DTO згенерованим ID для відповіді
            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RESTTeacher dto)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Age = dto.Age;
            existing.Salary = dto.Salary;
            existing.AcademicExperienceYears = dto.AcademicExperienceYears;

            if (!await _service.UpdateAsync(existing)) return BadRequest();
            await _service.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            if (!await _service.RemoveAsync(existing)) return BadRequest();
            await _service.SaveAsync();
            return NoContent();
        }
    }
}