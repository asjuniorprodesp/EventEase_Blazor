using EventEase.Api.Data;
using EventEase.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserSessionsController(EventEaseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserSession>>> GetAll()
    {
        return Ok(await dbContext.UserSessions.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserSession>> GetById(Guid id)
    {
        var entity = await dbContext.UserSessions.AsNoTracking().FirstOrDefaultAsync(e => e.SessionId == id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<UserSession>> Create(UserSession entity)
    {
        dbContext.UserSessions.Add(entity);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.SessionId }, entity);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UserSession entity)
    {
        if (id != entity.SessionId)
        {
            return BadRequest("O id da rota deve ser igual ao SessionId do corpo.");
        }

        var exists = await dbContext.UserSessions.AnyAsync(e => e.SessionId == id);
        if (!exists)
        {
            return NotFound();
        }

        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await dbContext.UserSessions.FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.UserSessions.Remove(entity);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
