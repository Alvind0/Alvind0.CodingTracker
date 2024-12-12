namespace Alvind0.CodingTracker.Models;

public class Enums
{
    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum MenuOption
    {
        // Main menu
        StartSession, ManuallyLog, Goals, CodingRecords, Exit,

        // Session Menu (5, 2)
        SessionStart, SessionEnd,

        // Goal Menu (7, 3)
        AddGoal, EditGoal, RemoveGoal,

        // Record Menu (10, 6)
        ViewRecords, EditRecord, DeleteRecord, SortRecords, FilterRecords, ShowReport,

        Return,
    }
}
