using System;
using SecureInventory.Auth;
using SecureInventory.Products;

class Program
{
    static void Main()
    {
        var authService = new AuthService();
        var productService = new ProductService();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Product Inventory App ===");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Option: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var user = authService.Login();
                    if (user != null)
                        MainMenu(user, productService);
                    break;
                case "2":
                    authService.Register();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Press Enter.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void MainMenu(User user, ProductService productService)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Welcome {user.Username} ({user.Role})");
            Console.WriteLine("1. View Products");
            if (user.Role == "admin")
            {
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Logout");
            }
            else
            {
                Console.WriteLine("2. Logout");
            }
            Console.Write("Option: ");
            var choice = Console.ReadLine();

            if (user.Role == "admin")
            {
                switch (choice)
                {
                    case "1": productService.ViewProducts(user); break;
                    case "2": productService.CreateProduct(user); break;
                    case "3": productService.UpdateProduct(user); break;
                    case "4": productService.DeleteProduct(user); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid option. Press Enter."); Console.ReadLine(); break;
                }
            }
            else
            {
                switch (choice)
                {
                    case "1": productService.ViewProducts(user); break;
                    case "2": return;
                    default: Console.WriteLine("Invalid option. Press Enter."); Console.ReadLine(); break;
                }
            }
        }
    }
}
