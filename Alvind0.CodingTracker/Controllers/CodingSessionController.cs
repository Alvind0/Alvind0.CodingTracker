using System.Globalization;
using Alvind0.CodingTracker.Data;
using Alvind0.CodingTracker.Models;
using Alvind0.CodingTracker.Utilities;
using Alvind0.CodingTracker.Views;
using Spectre.Console;
using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Controllers;

public class CodingSessionController
{
    private readonly CodingSessionRepository _repository;
    private readonly TableRenderer _tableRenderer = new();
    private readonly StopwatchHelper _stopwatchHelper = new();
    public CodingSessionController(CodingSessionRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException("Repository does not exist.");
    }

    public async Task RunStopwatch()
    {
        while (true)
        {
            var options = MenuHelper.GetStopwatchMenu(_stopwatchHelper.State);
            var selectedOption = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices<string>(options));

            switch (selectedOption)
            {
                case "Start":
                case "Resume":
                    _stopwatchHelper.StartStopwatch();
                    break;
                case "End":
                    _stopwatchHelper.StopStopwatch();
                    return;
            }
            //TODO: Figure out how long to delay
            await Task.Delay(100);
        }

    }

    public void LogSession(DateTime startTime, DateTime endTime)
    {

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
            var periodFilter = GetPeriodFilter();
            var sortType = GetSortType();
            var sortOrder = GetSortOrder();

            sessions = _repository.GetCodingSessions(periodFilter, sortType, sortOrder);
        }
        else sessions = _repository.GetCodingSessions();

        _tableRenderer.RenderSesionsTable(sessions);
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
                 .AddChoices<string>(SortingHelper.GetSortTypes()));

        return SortingHelper.GetSortTypeFromDescription(sortType);
    }

    internal SortOrder GetSortOrder()
    {
        var choices = Enum.GetValues<SortOrder>().Select(a => a.ToString()).ToList();
        var sortOrder = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Sort by order: ")
            .AddChoices<string>(choices));

        return Enum.TryParse<SortOrder>(sortOrder, out var result) ? result : SortOrder.Default;
    }

    internal PeriodFilter GetPeriodFilter()
    {
        var periodFilter = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Filter by period: ")
            .AddChoices<string>(SortingHelper.GetPeriodFilters()));

        return SortingHelper.GetPeriodFilterFromDescription(periodFilter);
    }

    internal void ShowReport()
    {
        var sessions = _repository.GetCodingSessions();
        var totalDuration = TimeSpan.Zero;
        foreach (var session in sessions)
        {
            totalDuration += session.Duration;
        }
        var sessionsCount = sessions.Count();
        var averageDuration = totalDuration / sessionsCount;

        _tableRenderer.RenderSessionsReport(sessionsCount, totalDuration, averageDuration);
    }
}
