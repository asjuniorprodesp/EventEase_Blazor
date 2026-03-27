using EventEase.Api.Data;
using EventEase.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventRegistrationsController(EventEaseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventRegistration>>> GetAll()
    {
        return Ok(await dbContext.EventRegistrations.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventRegistration>> GetById(int id)
    {
        var entity = await dbContext.EventRegistrations.AsNoTracking().FirstOrDefaultAsync(e => e.RegistrationId == id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<EventRegistration>> Create(EventRegistration entity)
    {
        dbContext.EventRegistrations.Add(entity);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.RegistrationId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EventRegistration entity)
    {
        if (id != entity.RegistrationId)
        {
            return BadRequest("O id da rota deve ser igual ao RegistrationId do corpo.");
        }

        var exists = await dbContext.EventRegistrations.AnyAsync(e => e.RegistrationId == id);
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
        var entity = await dbContext.EventRegistrations.FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.EventRegistrations.Remove(entity);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
