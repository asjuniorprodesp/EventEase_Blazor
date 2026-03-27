namespace EventEase.Api.Models;

public class Event
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string? EventDescription { get; set; }
    public DateOnly EventDate { get; set; }
    public string EventLocation { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
