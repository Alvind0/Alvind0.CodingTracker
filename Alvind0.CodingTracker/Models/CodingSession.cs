namespace Alvind0.CodingTracker.Models;

public class CodingSession
{
    public int Id { get; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration => (EndTime.HasValue ? EndTime.Value : DateTime.Now) - StartTime;
}