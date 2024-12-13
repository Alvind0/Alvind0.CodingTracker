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

var sessionRepository = new CodingSessionRepository(connectionString);
var goalRepository = new GoalRepository(connectionString);
var sessionController = new CodingSessionController(sessionRepository);
var goalController = new GoalController(goalRepository);
var menu = new Menu(sessionController);

sessionRepository.CreateTable();
goalRepository.CreateTable();
menu.MainMenu();
