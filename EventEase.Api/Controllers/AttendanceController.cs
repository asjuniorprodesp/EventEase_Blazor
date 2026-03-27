using EventEase.Api.Data;
using EventEase.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController(EventEaseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAll()
    {
        return Ok(await dbContext.Attendance.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Attendance>> GetById(int id)
    {
        var entity = await dbContext.Attendance.AsNoTracking().FirstOrDefaultAsync(e => e.AttendanceId == id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult<Attendance>> Create(Attendance entity)
    {
        dbContext.Attendance.Add(entity);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.AttendanceId }, entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Attendance entity)
    {
        if (id != entity.AttendanceId)
        {
            return BadRequest("O id da rota deve ser igual ao AttendanceId do corpo.");
        }

        var exists = await dbContext.Attendance.AnyAsync(e => e.AttendanceId == id);
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
        var entity = await dbContext.Attendance.FindAsync(id);
        if (entity is null)
        {
            return NotFound();
        }

        dbContext.Attendance.Remove(entity);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
