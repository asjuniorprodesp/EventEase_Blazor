using EventEase.Api.Data;
using EventEase.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(EventEaseDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        return Ok(await dbContext.Users.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetById(int id)
    {
        var user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.UserId)
        {
            return BadRequest("O id da rota deve ser igual ao UserId do corpo.");
        }

        var exists = await dbContext.Users.AnyAsync(u => u.UserId == id);
        if (!exists)
        {
            return NotFound();
        }

        dbContext.Entry(user).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
