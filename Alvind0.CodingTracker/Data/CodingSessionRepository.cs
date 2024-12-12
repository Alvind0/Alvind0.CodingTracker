using Dapper;
using Microsoft.Data.Sqlite;
using Alvind0.CodingTracker.Models;
using System.Net.NetworkInformation;
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

    public IEnumerable<CodingSession> GetCodingSessions()
    {
        using (var connection = GetConnection())
        {
            var query = @"SELECT * FROM 'Coding Sessions'";
            var sessions = connection.Query<CodingSession>(query);
            return sessions;
        }
    }
}
