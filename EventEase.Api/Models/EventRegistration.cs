namespace EventEase.Api.Models;

public class EventRegistration
{
    public int RegistrationId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime RegistrationDate { get; set; }
}
