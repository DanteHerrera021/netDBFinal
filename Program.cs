using NLog;
using System.Linq;
using FINAL.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

string path = Directory.GetCurrentDirectory() + "\\nlog.config";

var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logInfo("Program started");

try
{
    var db = new NWContext();
    string choice;
    do
    {
        ConsoleColor origColor;
        Console.WriteLine("1) Edit products");
        Console.WriteLine("2) Edit categories");
        Console.WriteLine("\"q\" to quit");
        choice = Console.ReadLine();
        Console.Clear();

        logInfo($"Option {choice} selected");

        if (choice == "1") // EDITING PRODUCTS (C)
        {

            origColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("EDITING PRODUCTS");
            Console.ForegroundColor = origColor;

            Console.WriteLine("1) Add new record to products");
            Console.WriteLine("2) Edit a product");
            Console.WriteLine("3) Display products");
            Console.WriteLine("4) Display specific product");
            choice = Console.ReadLine();
            Console.Clear();

            logInfo($"Option {choice} selected");
            if (choice == "1")
            {
                Product product = new Product();

                var productsById = db.Products.OrderBy(p => p.ProductId);
                product.ProductId = productsById.Last().ProductId + 1;

                Console.WriteLine("Enter the name of the product");
                product.ProductName = Console.ReadLine();

                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId);
                foreach (var item in suppliers)
                {
                    Console.WriteLine($"{item.SupplierId}) {item.CompanyName} - {item.ContactName}");
                }

                bool isValid = false;
                do
                {
                    Console.WriteLine("Please choose a supplier number to add your product to.");
                    string supId = Console.ReadLine();

                    if (int.TryParse(supId, out int selectedSupplierId))
                    {
                        if (suppliers.Any(s => s.SupplierId == selectedSupplierId))
                        {
                            product.SupplierId = selectedSupplierId;
                            isValid = !isValid;
                        }
                        else
                        {
                            origColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            logger.Error("Not a valid supplier ID.");
                            Console.ForegroundColor = origColor;
                        }
                    }
                    else
                    {
                        origColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        logger.Error("Not a valid number.");
                        Console.ForegroundColor = origColor;
                    }
                } while (!isValid);


                var categories = db.Categories.OrderBy(c => c.CategoryId);
                foreach (var item in categories)
                {
                    Console.WriteLine($"{item.CategoryId}) {item.CategoryName} - {item.Description}");
                }
                isValid = false;
                do
                {
                    Console.WriteLine("Please choose a category number to add your product to.");
                    string catId = Console.ReadLine();

                    if (int.TryParse(catId, out int selectedCategoryId))
                    {
                        if (categories.Any(c => c.CategoryId == selectedCategoryId))
                        {
                            product.CategoryId = selectedCategoryId;
                            isValid = !isValid;
                        }
                        else
                        {
                            origColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            logger.Error("Not a valid category ID.");
                            Console.ForegroundColor = origColor;
                        }
                    }
                    else
                    {
                        origColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        logger.Error("Not a valid number.");
                        Console.ForegroundColor = origColor;
                    }
                } while (!isValid);

                Console.WriteLine("Please enter the quantity per unit");
                product.QuantityPerUnit = Console.ReadLine();

                Console.WriteLine("Please enter the unit price as a decimal number");
                try
                {
                    product.UnitPrice = decimal.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                    logInfo("Exiting program, please try again.");
                    choice = "q";
                }

                Console.WriteLine("Enter the units in stock");
                product.UnitsInStock = short.Parse(Console.ReadLine());

                Console.WriteLine("Enter the units on order");
                product.UnitsOnOrder = short.Parse(Console.ReadLine());

                Console.WriteLine("Enter the reorder level");
                product.ReorderLevel = short.Parse(Console.ReadLine());

                Console.WriteLine("Is this product discontinued? Y/N");
                product.Discontinued = Console.ReadLine().ToUpper() == "Y";

                db.AddProduct(product);
            }
        }
        else if (choice == "2") // EDITING CATEGORIES (B)
        {

            origColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("EDITING CATEGORIES");
            Console.ForegroundColor = origColor;

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
    ConsoleColor origColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    logger.Error(e.Message);
    Console.ForegroundColor = origColor;
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