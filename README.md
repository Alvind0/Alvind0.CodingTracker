# Coding Tracker
This is a CRUD applications that logs the user's daily coding time. 
This is a project for [The C# Academy](https://thecsharpacademy.com/project/13/coding-tracker).
Developed using *C#*, *SQLite*, *Dapper ORM*, and *.NET MAUI*.

## Requirements 
- [x] Create a configuration files for database path and connection strings
- [x] Ability to log daily coding time.
- [x] Use *Spectre.Console* to show data
- [x] Have separate classes in different files
- [x] Create a `CodingSession` class which contains properties `Id`, `StartTime`, `EndTime`, `Duration`
- [ ] Session duration should be calculated automatically with `CalculateDuration` ??
- [x] Ability to input start and  end times manually
- [x] Use *Dapper ORM*
- [x] Read tables into a list of *Coding Sessions*
- [ ] Track coding time via a stopwatch(real-time)
- [ ] Ability to filter records per period(weeks, days, years)
- [ ] Ability to sort by ascending or descending
- [ ] Ability to set goals and show progress towards that goal(C# or SQL queries)
- [ ] Create a desktop app with *.NET MAUI*

## Features

## Lessons Learned
- Separating a project into multiple files was a challenge, but it helped make me code cleaner and easier to maintain.
- Models can map directly to database tables when using an *ORM*, simplifying the process

## Challenges