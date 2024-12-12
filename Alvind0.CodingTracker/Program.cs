using Microsoft.Extensions.Configuration;
using Alvind0.CodingTracker.Views;
using Alvind0.CodingTracker.Data;
using Alvind0.CodingTracker.Controllers;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("//Alvind0.CodingTracker//Properties//appsettings.json")
    .Build();

var connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"] 
    ?? throw new NullReferenceException("Connection String does not exist in the configuration file.");

var database = new CodingSessionRepository(connectionString);
var sessionController = new CodingSessionController(database);
var menu = new Menu(sessionController);
database.CreateTables();
menu.MainMenu();
