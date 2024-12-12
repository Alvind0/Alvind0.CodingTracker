using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Models;

public class SortCriteria
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SortOrder Sort { get; set; } = SortOrder.Ascending;
}


