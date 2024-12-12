using Alvind0.CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
namespace Alvind0.CodingTracker.Data;


public class CodingSessionRepository
{
    private readonly string _connectionString;

    public CodingSessionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    // Initialize tables: "Coding Sessions" & "Goals" If it doesn't exist yet 
    public void CreateTables()
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            var query = @"
            CREATE TABLE IF NOT EXISTS 'Coding Sessions'(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            StartTime TEXT NOT NULL,
            EndTime TEXT NOT NULL,
            Duration INTEGER);
            CREATE TABLE IF NOT EXISTS Goals(
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Goal INTEGER NOT NULL,
            StartDate TEXT NOT NULL,
            EndDate TEXT,
            Progress INTEGER,
            IsCompleted INTEGER);
            ";

            connection.Execute(query);
        }
    }

    public void AddSession(DateTime start, DateTime end)
    {
        var session = new CodingSession
        {
            StartTime = start,
            EndTime = end
        };

        using (var connection = GetConnection())
        {
            connection.Open();
            var query = @"
INSERT INTO 'Coding Sessions' (StartTime, EndTime, Duration) 
VALUES (@StartTime, @EndTime, @Duration);";
            connection.Execute(query, session);
        }
    }

    //TODO: Implement
    public void UpdateSession(int id, bool isUpdateStart, bool isUpdateEnd, DateTime? startTime = null, DateTime? endTime = null)
    {
        var query = "";

        if (!isUpdateStart && !isUpdateEnd)
        {
            Console.WriteLine("Nothing to update here.");
            return;
        }

        using (var connection = GetConnection())
        {
            connection.Open();
            if (!startTime.HasValue)
            {
                var databaseStartTime = @"SELECT StartTime FROM 'Coding Sessions' WHERE Id = @Id";
                startTime = connection.QuerySingle<DateTime>(databaseStartTime, id);
            }

            if (!endTime.HasValue)
            {
                var databaseEndTime = @"SELECT EndTime FROM 'Coding Sessions' WHERE Id = @Id";
                endTime = connection.QuerySingle<DateTime>(databaseEndTime, id);
            }

            var session = new CodingSession
            {
                Id = id,
                StartTime = startTime.HasValue ? startTime.Value : throw new Exception("StartTime has no value."),
                EndTime = endTime
            };

            if (isUpdateStart && isUpdateEnd) query = @"
UPDATE 'Coding Sessions' SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";

            connection.Execute(query, session);
        }

    }

    public IEnumerable<CodingSession> GetCodingSessions()
    {
        using (var connection = GetConnection())
        {
            var query = @"SELECT * FROM 'Coding Sessions'";
            var sessions = connection.Query<CodingSession>(query);
            return sessions;
        }
    }

    internal bool VerifyId(int id)
    {
        using (var connection = GetConnection())
        {
            var query = @"Select Id FROM 'Coding Sessions' WHERE Id = @Id";
            var isExists = connection.QuerySingleOrDefault<int?>(query, new { Id = id });
            return isExists != null ? true : false;
        }
    }
}
