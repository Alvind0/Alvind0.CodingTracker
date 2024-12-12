using static Alvind0.CodingTracker.Models.Enums;
namespace Alvind0.CodingTracker.Utilities;

public class MenuHelper
{
    public static readonly Dictionary<MenuOption, string> MenuOptionDescriptions = new()
    {
        { MenuOption.StartSession, "Start Coding Session" },
        { MenuOption.ManuallyLog, "Add Session Manually" },
        { MenuOption.Goals, "Goals" },
        { MenuOption.CodingRecords, "Coding Records" },
        { MenuOption.Exit, "Exit Application" },

        { MenuOption.SessionStart, "Start Session" },
        { MenuOption.SessionEnd, "End Session" },

        { MenuOption.AddGoal, "Add Goal" },
        { MenuOption.EditGoal, "Edit Goal" },
        { MenuOption.RemoveGoal, "Remove Goal" },

        { MenuOption.ViewRecords, "View Records" },
        { MenuOption.EditRecord, "Edit Record" },
        { MenuOption.DeleteRecord, "Delete Record" },
        { MenuOption.SortRecords, "Sort Records" },
        { MenuOption.FilterRecords, "Filter Records" },
        { MenuOption.ShowReport, "Show Report" },

        { MenuOption.Return, "Go Back" },
    };

    public static string GetMenuOption(MenuOption option)
    {
        var result = "";
        return MenuOptionDescriptions.TryGetValue(option, out result) ? result : throw new NullReferenceException("Menu option does not exist.");
    }
    public static List<string> GetMenuOptions(int startIndex, int count)
        => MenuOptionDescriptions.Values.ToList().Skip(startIndex).Take(count).ToList();

    public static MenuOption GetMenuOptionFromDescription(string description)
    {
        foreach (var option in MenuOptionDescriptions)
        {
            if (option.Value == description)
            {
                return option.Key;
            }
        }
        // Compliler say option may not exist. Green squiggly line annoy me. I throw exception. Me smart.
        throw new NullReferenceException($"Invalid menu description: {description}");
    }
}

