namespace Alvind0.CodingTracker.Models;
// TODO : Create logic for Progress to be automatically calculated
public class Goal
{
    public int Id { get; set; }
    public string Name { get; set; } = "Undefined";
    public TimeSpan DurationGoal { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan Progress { get; set; } = TimeSpan.Zero;
    public bool IsCompleted => Progress >= DurationGoal;
}