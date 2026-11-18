using Microsoft.AspNetCore.Mvc;
using University.Common;
using DBClassroom = University.Infrastructure.Models.ClassroomModel;
using RESTClassroom = University.REST.Models.ClassroomDto;

namespace University.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomController : ControllerBase
    {
        private readonly ICrudServiceAsync<DBClassroom> _service;

        public ClassroomController(ICrudServiceAsync<DBClassroom> service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RESTClassroom>>> GetAll()
        {
            var items = await _service.ReadAllAsync();
            return Ok(items.Select(c => new RESTClassroom
            {
                Id = c.Id,
                RoomNumber = c.RoomNumber,
                Capacity = c.Capacity,
                HasProjector = c.HasProjector,
                TeacherId = c.TeacherId
            }));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RESTClassroom>> GetById(Guid id)
        {
            var c = await _service.ReadAsync(id);
            if (c == null) return NotFound();
            return Ok(new RESTClassroom
            {
                Id = c.Id,
                RoomNumber = c.RoomNumber,
                Capacity = c.Capacity,
                HasProjector = c.HasProjector,
                TeacherId = c.TeacherId
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RESTClassroom dto)
        {
            var entity = new DBClassroom
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                RoomNumber = dto.RoomNumber,
                Capacity = dto.Capacity,
                HasProjector = dto.HasProjector,
                TeacherId = dto.TeacherId
            };
            if (!await _service.CreateAsync(entity)) return BadRequest();
            await _service.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] RESTClassroom dto)
        {
            var existing = await _service.ReadAsync(id);
            if (existing == null) return NotFound();

            existing.RoomNumber = dto.RoomNumber;
            existing.Capacity = dto.Capacity;
            existing.HasProjector = dto.HasProjector;
            existing.TeacherId = dto.TeacherId;

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