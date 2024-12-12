using Alvind0.CodingTracker.Utilities;
using Alvind0.CodingTracker.Controllers;
using Spectre.Console;
using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Views;

public class Menu
{
    private readonly CodingSessionController _codingSessionController;
    public Menu(CodingSessionController codingSessionController)
    {
        _codingSessionController = codingSessionController;
    }
    public void MainMenu()
    {
        var isExitApp = false;
        while (true)
        {
            Console.Clear();
            var menuOptions = MenuHelper.GetMenuOptions(0, 5);
            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose a menu: ")
                .AddChoices<string>(menuOptions));

            var selectedOption = MenuHelper.GetMenuOptionFromDescription(userChoice);
            switch (selectedOption)
            {
                case MenuOption.StartSession:
                    SubMenu(5, 2);
                    break;
                case MenuOption.ManuallyLog:
                    _codingSessionController.LogSessionManually();
                    break;
                case MenuOption.Goals:
                    SubMenu(7, 3);
                    break;
                case MenuOption.CodingRecords:
                    SubMenu(10, 6);
                    break;
                case MenuOption.Exit:
                    Console.WriteLine("Goodbye. ");
                    isExitApp = true;
                    break;
                default:
                    break;
            }

            if (isExitApp) break;
        }
    }

    private void SubMenu(int startIndex, int count)
    {
        var isReturnToMainMenu = false;
        while (true)
        {
            var menuOptions = MenuHelper.GetMenuOptions(startIndex, count);
            var returnMenu = MenuHelper.GetMenuOption(MenuOption.Return);
            menuOptions.Add(returnMenu);

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Choose a menu: ")
                .AddChoices<string>(menuOptions));

            var selectedOption = MenuHelper.GetMenuOptionFromDescription(userChoice);
            switch (selectedOption)
            {
                case MenuOption.SessionStart:
                    break;
                case MenuOption.SessionEnd:
                    break;
                case MenuOption.AddGoal:
                    break;
                case MenuOption.EditGoal:
                    break;
                case MenuOption.RemoveGoal:
                    break;
                case MenuOption.ViewRecords:
                    _codingSessionController.ShowCodingSessions();
                    break;
                case MenuOption.EditRecord:
                    break;
                case MenuOption.DeleteRecord:
                    break;
                case MenuOption.SortRecords:
                    break;
                case MenuOption.FilterRecords:
                    break;
                case MenuOption.ShowReport:
                    break;
                case MenuOption.Return:
                    isReturnToMainMenu = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
            if (isReturnToMainMenu) break;
        }
    }
}