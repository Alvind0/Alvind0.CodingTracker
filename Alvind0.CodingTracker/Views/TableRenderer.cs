using Alvind0.CodingTracker.Models;
using Spectre.Console;

namespace Alvind0.CodingTracker.Views;

public class TableRenderer
{
    public void RenderGoalsTable(IEnumerable<Goal> goals)
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumns("Id", "Name", "Goal", "Start Date", "Deadline", "Progress", "Completed");
        table.Columns[0].RightAligned();
        table.Columns[6].LeftAligned();

        foreach (var goal in goals)
        {
            table.AddRow(
                goal.Id.ToString(),
                goal.Name,
                goal.StartDate.ToString("MM-dd-yy"),
                goal.EndDate?.ToString("MM-dd-yy") ?? "Invalid Date.",
                goal.Progress.ToString(@"h\:mm"),
                goal.IsCompleted ? "Completed" : "Incomplete");
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(table);
    }

    public void RenderSesionsTable(IEnumerable<CodingSession> sessions)
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumns("Id", "Start Time", "End Time", "Duration");

        foreach (var session in sessions)
        {
            table.AddRow(
                session.Id.ToString(),
                session.StartTime.ToString("MM-dd-yy H:mm"),
                session.EndTime?.ToString("MM-dd-yy H:mm") ?? "Invalid Time.",
                session.Duration.ToString(@"hh\:mm")
                );
        }
        Console.Clear();
        AnsiConsole.Write(table);
    }

    public void RenderSessionsReport(int sessionsCount, TimeSpan totalDuration, TimeSpan averageDuration)
    {
        var table = new Table();
        table.Border = TableBorder.Rounded;

        table.AddColumns("Session Count", "Total Coding Time", "Average Cooding Time");
        table.Columns[0].RightAligned();
        table.Columns[1].Centered();
        table.Columns[2].Centered();

        table.AddRow(sessionsCount.ToString(), totalDuration.ToString(@"hh\:mm"), averageDuration.ToString(@"hh\:mm"));

        Console.Clear();
        AnsiConsole.Write(table);
    }
}
