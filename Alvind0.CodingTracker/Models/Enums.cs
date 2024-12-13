namespace Alvind0.CodingTracker.Models;

public class Enums
{
    public enum SortOrder
    {
        Default, Ascending, Descending
    }

    public enum SortType
    {
        Default, Id, Date, Duration
    }

    public enum MenuOption
    {
        // Main menu
        StartSession, ManuallyLog, Goals, CodingRecords, Exit,

        // Session Menu (5, 2)
        SessionStart, SessionEnd,

        // Goal Menu (7, 4)
        ViewGoals, AddGoal, EditGoal, RemoveGoal, 

        // Record Menu (11, 6)
        ViewRecords, EditRecord, DeleteRecord, SortRecords, FilterRecords, ShowReport,

        // 17
        Return,
    }
}
