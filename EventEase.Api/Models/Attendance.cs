namespace EventEase.Api.Models;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public int EventId { get; set; }
    public DateTime CheckInTime { get; set; }
}
