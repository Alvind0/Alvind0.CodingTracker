using Alvind0.CodingTracker.Models;
using static Alvind0.CodingTracker.Models.Enums;
using Dapper;

namespace Alvind0.CodingTracker.Data;


public class CodingSessionRepository : Repository
{
    public CodingSessionRepository(string connectionString) : base(connectionString) { }

    // Initialize tables: "Coding Sessions" & "Goals" If it doesn't exist yet 
    public void CreateTable()
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

    public void UpdateSession(int id, bool isUpdateStart, bool isUpdateEnd, DateTime? startTime = null, DateTime? endTime = null)
    {
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

            var query = @"
UPDATE 'Coding Sessions' SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";

            connection.Execute(query, session);
        }

    }

    public void DeleteSession(int id)
    {
        using (var connection = GetConnection())
        {
            var query = @"DELETE FROM 'Coding Sessions' WHERE Id = @Id";
            connection.Execute(query, id);
        }
    }

    public IEnumerable<CodingSession> GetCodingSessions(SortType sortType = SortType.Default, SortOrder sortOrder = SortOrder.Default)
    {
        string query = "";
        
        using (var connection = GetConnection())
        {
            switch (sortType, sortOrder)
            {
                case (SortType.Id, SortOrder.Ascending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY Id ASC";
                    break;
                case (SortType.Id, SortOrder.Descending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY Id DESC";
                    break;
                case (SortType.Date, SortOrder.Ascending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY StartTime ASC";
                    break;
                case (SortType.Date, SortOrder.Descending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY StartTime DESC";
                    break;
                case (SortType.Duration, SortOrder.Ascending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY Duration ASC";
                    break;
                case (SortType.Duration, SortOrder.Descending):
                    query = @"SELECT * FROM 'Coding Sessions' ORDER BY Duration DESC";
                    break;
                default:
                    query = @"SELECT * FROM 'Coding Sessions' ";
                    break;
            }
            var sessions = connection.Query<CodingSession>(query);
            return sessions;
        }
    }

    internal bool VerifyIfIdExists(int id)
    {
        using (var connection = GetConnection())
        {
            var query = @"Select Id FROM 'Coding Sessions' WHERE Id = @Id";
            var isExists = connection.QuerySingleOrDefault<int?>(query, new { Id = id });
            return isExists != null ? true : false;
        }
    }
}
