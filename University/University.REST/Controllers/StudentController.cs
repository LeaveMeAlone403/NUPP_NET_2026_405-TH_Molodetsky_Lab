using Microsoft.AspNetCore.Mvc;
using University.Common;
using DBStudent = University.Infrastructure.Models.StudentModel;
using RESTStudent = University.REST.Models.StudentDto;

namespace University.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICrudServiceAsync<DBStudent> _service;

        public StudentController(ICrudServiceAsync<DBStudent> service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RESTStudent>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items.Select(s => new RESTStudent
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                Group = s.Group,
                LastGrade = s.LastGrade,
                AverageScore = s.AverageScore,
                EnrollmentIds = s.Enrollments?.Select(e => e.Id).ToList() ?? new List<Guid>()
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RESTStudent>> GetById(Guid id)
        {
            var s = await _service.ReadAsync(id);
            if (s == null) return NotFound();
            return Ok(new RESTStudent
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                Group = s.Group,
                LastGrade = s.LastGrade,
                AverageScore = s.AverageScore,
                EnrollmentIds = s.Enrollments?.Select(e => e.Id).ToList() ?? new List<Guid>()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RESTStudent dto)
        {
            var entity = new DBStudent
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Name = dto.Name,
                Age = dto.Age,
                Group = dto.Group,
                LastGrade = dto.LastGrade,
                AverageScore = dto.AverageScore
            };
            if (!await _service.CreateAsync(entity)) return BadRequest();
            await _service.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RESTStudent dto)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            existing.Age = dto.Age;
            existing.Group = dto.Group;
            existing.LastGrade = dto.LastGrade;
            existing.AverageScore = dto.AverageScore;
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