namespace EventEase_Blazor.Models;

public class AttendanceItem
{
    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime CheckInTime { get; set; }
}