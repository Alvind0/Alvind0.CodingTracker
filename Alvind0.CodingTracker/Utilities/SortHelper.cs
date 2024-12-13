using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Utilities;

public class SortHelper
{
    public static readonly Dictionary<SortType, string> SortTypes = new()
    {
        { SortType.Default, "Default" },
        { SortType.Id, "Sort by ID" },
        { SortType.Date, "Sort by Date" },
        { SortType.Duration, "Sort by Duration" }
    };

    public static List<string> GetSortTypes() => SortTypes.Values.ToList();

    public static SortType GetSortTypeFromDescription(string description)
    {
        foreach (var sortType in SortTypes)
        {
            if (sortType.Value == description) return sortType.Key;
        }
        throw new NullReferenceException("Invalid sort type: " + description);
    }
}
