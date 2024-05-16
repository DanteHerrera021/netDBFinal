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

                // Attempted to assign a new ID but it threw an error
                // var productsById = db.Products.OrderBy(p => p.ProductId);
                // product.ProductId = productsById.Last().ProductId + 1;

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

                logInfo($"{product.ProductName} was successfully added to the database");
            }
            else if (choice == "2")
            {

                Product product;

                var products = db.Products.OrderBy(p => p.ProductId);
                foreach (var item in products)
                {
                    Console.WriteLine($"{item.ProductId}) {item.ProductName}");
                }
                bool isValid = false;
                do
                {
                    Console.WriteLine("Please choose a product number.");
                    string productId = Console.ReadLine();

                    product = products.FirstOrDefault(p => p.ProductId == int.Parse(productId));

                    if (int.TryParse(productId, out int productIdInt))
                    {
                        product = products.FirstOrDefault(p => p.ProductId == productIdInt);

                        if (product == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Not a valid product ID.");
                            Console.ResetColor();
                        }
                        else
                        {
                            isValid = true;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid number.");
                        Console.ResetColor();
                    }
                } while (!isValid);

                Console.Clear();

                Console.WriteLine("Choose an attribute to edit");
                Console.WriteLine("1) Product Name");
                Console.WriteLine("2) Quantity per unit");
                Console.WriteLine("3) Unit price");
                Console.WriteLine("4) Units in stock");
                Console.WriteLine("5) Units on order");
                Console.WriteLine("6) Reorder level");
                Console.WriteLine("7) Change discontinued status");
                Console.WriteLine($"   Current status: {product.Discontinued}");

                string editChoice = Console.ReadLine();
                logInfo($"Option {choice} selected");

                switch (editChoice)
                {
                    case "1":
                        Console.WriteLine("Enter new product name:");
                        product.ProductName = Console.ReadLine();
                        break;
                    case "2":
                        Console.WriteLine("Enter new quantity per unit:");
                        product.QuantityPerUnit = Console.ReadLine();
                        break;
                    case "3":
                        Console.WriteLine("Enter new unit price:");
                        if (decimal.TryParse(Console.ReadLine(), out decimal unitPrice))
                        {
                            product.UnitPrice = unitPrice;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for unit price.");
                        }
                        break;
                    case "4":
                        Console.WriteLine("Enter new units in stock:");
                        if (short.TryParse(Console.ReadLine(), out short unitsInStock))
                        {
                            product.UnitsInStock = unitsInStock;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for units in stock.");
                        }
                        break;
                    case "5":
                        Console.WriteLine("Enter new units on order:");
                        if (short.TryParse(Console.ReadLine(), out short unitsOnOrder))
                        {
                            product.UnitsOnOrder = unitsOnOrder;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for units on order.");
                        }
                        break;
                    case "6":
                        Console.WriteLine("Enter new reorder level:");
                        if (short.TryParse(Console.ReadLine(), out short reorderLevel))
                        {
                            product.ReorderLevel = reorderLevel;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for reorder level.");
                        }
                        break;
                    case "7":
                        Console.WriteLine("Change discontinued status (true/false):");
                        if (bool.TryParse(Console.ReadLine(), out bool discontinued))
                        {
                            product.Discontinued = discontinued;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input for discontinued status.");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                db.SaveChanges();
                logInfo($"{product.ProductName} was successfully updated");
            }
            else if (choice == "3")
            {

                Console.WriteLine("Which products would you like to display");
                Console.WriteLine("1) Active products");
                Console.WriteLine("2) Discontinued products");
                Console.WriteLine("3) All products");

                choice = Console.ReadLine();
                logInfo($"Option {choice} selected");

                if (choice == "1")
                {
                    var products = db.Products.Where(p => p.Discontinued == false).OrderBy(p => p.ProductId);

                    foreach (var item in products)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }
                else if (choice == "2")
                {
                    var products = db.Products.Where(p => p.Discontinued == true).OrderBy(p => p.ProductId);

                    foreach (var item in products)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }
                else if (choice == "3")
                {
                    var products = db.Products.OrderBy(p => p.ProductId);

                    foreach (var item in products)
                    {
                        Console.WriteLine(item.ProductName);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    logger.Error("Invalid choice");
                    Console.ResetColor();
                }
            }
            else if (choice == "4")
            {
                Product product;

                var products = db.Products.OrderBy(p => p.ProductId);
                foreach (var item in products)
                {
                    Console.WriteLine($"{item.ProductId}) {item.ProductName}");
                }
                bool isValid = false;
                do
                {
                    Console.WriteLine("Please choose a product number.");
                    string productId = Console.ReadLine();

                    product = products.FirstOrDefault(p => p.ProductId == int.Parse(productId));

                    if (int.TryParse(productId, out int productIdInt))
                    {
                        product = products.FirstOrDefault(p => p.ProductId == productIdInt);

                        if (product == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Not a valid product ID.");
                            Console.ResetColor();
                        }
                        else
                        {
                            isValid = true;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please enter a valid number.");
                        Console.ResetColor();
                    }
                } while (!isValid);

                Console.WriteLine($"{product.ProductId}) {product.ProductName}");
                Console.WriteLine($"{product.SupplierId}) {db.Suppliers.FirstOrDefault(s => s.SupplierId == product.SupplierId).CompanyName}");
                Console.WriteLine($"{product.CategoryId}) {db.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId).CategoryName}");
                Console.WriteLine($"Quantity per unit: {product.QuantityPerUnit}");
                Console.WriteLine($"Unit Price: ${product.UnitPrice}");
                Console.WriteLine($"Units in stock: {product.UnitsInStock}");
                Console.WriteLine($"Units on order: {product.UnitsOnOrder}");
                Console.WriteLine($"Reorder level: {product.ReorderLevel}");
                Console.WriteLine($"Product discontinued: {product.Discontinued}");
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

            Console.WriteLine("Not attempted.");
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
