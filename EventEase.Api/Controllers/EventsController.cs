using EventEase.Api.Data;
using EventEase.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(EventEaseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAll()
    {
        return Ok(await dbContext.Events.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Event>> GetById(int id)
    {
        var entity = await dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.EventId == id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Event>> Create(Event entity)
    {
        dbContext.Events.Add(entity);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.EventId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Event entity)
    {
        if (id != entity.EventId)
        {
            return BadRequest("O id da rota deve ser igual ao EventId do corpo.");
        }

        var exists = await dbContext.Events.AnyAsync(e => e.EventId == id);
        if (!exists)
        {
            return NotFound();
        }

        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await dbContext.Events.FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Events.Remove(entity);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
