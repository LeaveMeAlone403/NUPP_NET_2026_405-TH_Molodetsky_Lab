using Microsoft.AspNetCore.Mvc;
using University.Common;
using DBCourse = University.Infrastructure.Models.CourseModel;
using RESTCourse = University.REST.Models.CourseDto;

namespace University.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICrudServiceAsync<DBCourse> _service;

        public CourseController(ICrudServiceAsync<DBCourse> service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RESTCourse>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items.Select(c => new RESTCourse
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits,
                TeacherId = c.TeacherId // Мапінг TeacherId для читання
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RESTCourse>> GetById(Guid id)
        {
            var c = await _service.ReadAsync(id);
            if (c == null) return NotFound();
            return Ok(new RESTCourse
            {
                Id = c.Id,
                Title = c.Title,
                Credits = c.Credits,
                TeacherId = c.TeacherId // Мапінг TeacherId для читання
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RESTCourse dto)
        {
            var entity = new DBCourse
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Title = dto.Title,
                Credits = dto.Credits,
                TeacherId = dto.TeacherId // Зберігаємо TeacherId при створенні
            };
            if (!await _service.CreateAsync(entity)) return BadRequest();
            await _service.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RESTCourse dto)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();
            existing.Title = dto.Title;
            existing.Credits = dto.Credits;
            existing.TeacherId = dto.TeacherId; // Оновлюємо TeacherId
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