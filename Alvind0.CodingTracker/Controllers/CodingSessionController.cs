using System.Globalization;
using Alvind0.CodingTracker.Data;
using Spectre.Console;
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
            if (startTime > endTime)
            {
                Console.Clear();
                Console.WriteLine("Start Time cannot be later than End Time");
                continue;
            }
            break;
        }
        _repository.AddSession(startTime, endTime);
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

    public void ShowCodingSessions()
    {
        var sessions = _repository.GetCodingSessions();

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

        AnsiConsole.Write(table);
    }
}
