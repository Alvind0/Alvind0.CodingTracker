using System.Globalization;
using Alvind0.CodingTracker.Data;
using Spectre.Console;
using Alvind0.CodingTracker.Utilities;
using Alvind0.CodingTracker.Models;
using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Controllers;

public class CodingSessionController
{
    private readonly CodingSessionRepository _repository;

    public CodingSessionController(CodingSessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException("Repository does not exist.");
    }


    public void LogSessionManually()
    {
        DateTime startTime, endTime;
        while (true)
        {
            startTime = GetTime("Insert Start Time (Format: MM-dd-yy H:mm): ");
            endTime = GetTime("Insert End Time (Format: MM-dd-yy H:mm): ");
            if (ValidateDateTime(startTime, endTime)) break;
        }
        _repository.AddSession(startTime, endTime);
    }

    public void EditSession()
    {
        var isUpdateStart = false;
        var isUpdateEnd = false;
        DateTime startTime, endTime;
        var id = GetId("Enter the Id for the session do you want to edit.");
        var updateItems = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("Choose items to edit: ")
            .InstructionsText(
                "(Press [blue]<space>[/] to toggle options and [green]<enter>[/] to accept.")
            .AddChoices(new[]
            {
                "Start time", "End time"
            }));


        if (updateItems.Contains("Start time") && updateItems.Contains("End time"))
        {
            isUpdateStart = true;
            isUpdateEnd = true;

            while (true)
            {
                startTime = GetTime(@"Enter start time (format: MM-dd-yy H:mm)");
                endTime = GetTime(@"Enter end time (format: MM-dd-yy H:mm)");
                if (ValidateDateTime(startTime, endTime)) break;
            }
            _repository.UpdateSession(id, isUpdateStart, isUpdateEnd, startTime, endTime);
        }
        else if (updateItems.Contains("Start time"))
        {
            isUpdateStart = true;
            startTime = GetTime(@"Enter start time (format: MM-dd-yy H:mm)");
            _repository.UpdateSession(id, isUpdateStart, isUpdateEnd, startTime);
        }
        else if (updateItems.Contains("End time"))
        {
            isUpdateEnd = true;
            endTime = GetTime(@"Enter end time (format: MM-dd-yy H:mm)");
            _repository.UpdateSession(id, isUpdateStart, isUpdateEnd, endTime);
        }
    }

    private int GetId(string message)
    {
        ShowCodingSessions();
        while (true)
        {
            var id = AnsiConsole.Ask<int>(message);
            var isExists = _repository.VerifyIfIdExists(id);
            if (isExists) return id;
            Console.WriteLine("Record does not exists.");
        }
    }

    public DateTime GetTime(string message)
    {
        while (true)
        {
            var time = AnsiConsole.Ask<string>(message);
            if (DateTime.TryParseExact(time, "MM-dd-yy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }
            Console.Clear();
            Console.WriteLine("Please enter in the correct format.");
        }
    }

    public bool ValidateDateTime(DateTime startTime, DateTime endTime)
    {
        if (startTime > endTime)
        {
            Console.Clear();
            Console.WriteLine("Start time cannot be later than End time");
            return false;
        }
        return true;
    }

    public void ShowCodingSessions(bool isOnlyView = false)
    {
        IEnumerable<CodingSession> sessions;
        if (isOnlyView)
        {
            var sortType = GetSortType();
            var sortOrder = GetSortOrder();
            sessions = _repository.GetCodingSessions(sortType, sortOrder);
        }
        else sessions = _repository.GetCodingSessions();

        var table = new Table();
        table.Border = TableBorder.Rounded;
        table.AddColumns("Id", "Start Time", "EndTime", "Duration");

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

    internal void DeleteSession()
    {
        ShowCodingSessions();
        var id = GetId("Enter the Id of the session you want to delete.");
        _repository.DeleteSession(id);
    }

    internal SortType GetSortType()
    {
        var sortType = AnsiConsole.Prompt(
                 new SelectionPrompt<string>()
         .Title("Sort by type: ")
                 .AddChoices<string>(SortHelper.GetSortTypes()));

        return SortHelper.GetSortTypeFromDescription(sortType);
    }

    internal SortOrder GetSortOrder()
    {
        var choices = Enum.GetValues(typeof(SortOrder))
    .Cast<SortOrder>()
    .Select(order => order.ToString())
    .ToList();
        var sortOrder = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Sort by order: ")
            .AddChoices<string>(choices));

        return Enum.TryParse<SortOrder>(sortOrder, out var result) ? result : SortOrder.Default ;
    }
}
