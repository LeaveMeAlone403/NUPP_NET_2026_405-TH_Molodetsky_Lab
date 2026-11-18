using Microsoft.AspNetCore.Mvc;
using University.Common;
using DBEnrollment = University.Infrastructure.Models.EnrollmentModel;
using RESTEnrollment = University.REST.Models.EnrollmentDto;

namespace University.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly ICrudServiceAsync<DBEnrollment> _service;

        public EnrollmentController(ICrudServiceAsync<DBEnrollment> service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RESTEnrollment>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items.Select(e => new RESTEnrollment
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                Grade = e.Grade ?? 0 // Безпечна обробка null, якщо оцінки ще немає
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RESTEnrollment>> GetById(Guid id)
        {
            var e = await _service.ReadAsync(id);
            if (e == null) return NotFound();
            return Ok(new RESTEnrollment
            {
                Id = e.Id,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                Grade = e.Grade ?? 0
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RESTEnrollment dto)
        {
            var entity = new DBEnrollment
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Grade = dto.Grade
            };
            if (!await _service.CreateAsync(entity)) return BadRequest();
            await _service.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RESTEnrollment dto)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();
            existing.StudentId = dto.StudentId;
            existing.CourseId = dto.CourseId;
            existing.Grade = dto.Grade;
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