using NLog;
using System.Linq;
// using NWConsole.Model;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logInfo("Program started");

try
{
    string choice;
    do
    {
        Console.WriteLine("1) Edit products");
        Console.WriteLine("2) Edit categories");
        Console.WriteLine("\"q\" to quit");
        choice = Console.ReadLine();
        Console.Clear();

        logInfo($"Option {choice} selected");

        if (choice == "1") // EDITING PRODUCTS (C)
        {

            Console.WriteLine("1) Add new record to products");
            Console.WriteLine("2) Edit a product");
            Console.WriteLine("3) Display products");
            Console.WriteLine("4) Display specific product");
            choice = Console.ReadLine();
            Console.Clear();

            logInfo($"Option {choice} selected");
        }
        else if (choice == "2") // EDITING CATEGORIES (B)
        {
            Console.WriteLine("1) Add new record to categories");
            Console.WriteLine("2) Edit a category");
            Console.WriteLine("3) Display all categories");
            Console.WriteLine("4) Display all categories' active products");
            Console.WriteLine("5) Display specific category's active products");
            choice = Console.ReadLine();
            Console.Clear();

            logInfo($"Option {choice} selected");
        }
    } while (choice.ToLower() != "q");

}
catch (Exception e)
{
    logger.Error(e.Message);
}

logInfo("Program ended");

static void logInfo(string message)
{
    string path = Directory.GetCurrentDirectory() + "\\nlog.config";

    var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

    ConsoleColor origColor = Console.ForegroundColor;

    Console.ForegroundColor = ConsoleColor.Green;
    logger.Info(message);
    Console.ForegroundColor = origColor;
}