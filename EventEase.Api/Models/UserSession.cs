namespace EventEase.Api.Models;

public class UserSession
{
    public Guid SessionId { get; set; }
    public int UserId { get; set; }
    public string? SessionData { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
