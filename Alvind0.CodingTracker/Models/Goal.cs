namespace Alvind0.CodingTracker.Models;

public class Goal
{
    public int Id { get; }
    public TimeSpan DurationGoal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Progress { get; set; }
    public bool IsCompleted => Progress >= DurationGoal;
}

