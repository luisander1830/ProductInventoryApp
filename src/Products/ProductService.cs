using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SecureInventory.Auth;
using SecureInventory.Logging;

namespace SecureInventory.Products;

public class ProductService
{
    private List<Product> products = new List<Product>();
    private string filePath = "data/products.json";

    public ProductService()
    {
        Load();
    }

    private void Load()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            products = JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }
    }

    private void Save()
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(products, Formatting.Indented));
    }

    public void ViewProducts(User user)
    {
        Console.Clear();
        Console.WriteLine("=== Products ===");
        foreach (var p in products)
        {
            Console.WriteLine($"ID:{p.Id} Name:{p.Name} SKU:{p.Sku} Qty:{p.Qty} Price:{p.Price} Category:{p.Category} Active:{p.Active}");
        }
        Logger.Log(user.Id, user.Role, "VIEW", "Product", 0, "SUCCESS");
        Console.WriteLine("Press Enter.");
        Console.ReadLine();
    }

    public void CreateProduct(User user)
    {
        if (user.Role != "admin")
        {
            Console.WriteLine("Unauthorized. Press Enter.");
            Console.ReadLine();
            return;
        }

        Console.Write("Name (3-60 chars): ");
        string name = Console.ReadLine() ?? "";
        if (name.Length < 3 || name.Length > 60)
        {
            Console.WriteLine("Invalid length. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write("SKU: ");
        string sku = Console.ReadLine() ?? "";
        if (products.Any(p => p.Sku == sku))
        {
            Console.WriteLine("SKU exists. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write("Qty >=0: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
        {
            Console.WriteLine("Invalid qty. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write("Price >=0: ");
        if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
        {
            Console.WriteLine("Invalid price. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write("Category: ");
        string category = Console.ReadLine() ?? "";

        var product = new Product
        {
            Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1,
            Name = name,
            Sku = sku,
            Qty = qty,
            Price = price,
            Category = category,
            Active = true,
            CreatedAt = DateTime.Now
        };

        products.Add(product);
        Save();
        Logger.Log(user.Id, user.Role, "CREATE", "Product", product.Id, "SUCCESS");
        Console.WriteLine("Product created. Press Enter.");
        Console.ReadLine();
    }

    public void UpdateProduct(User user)
    {
        if (user.Role != "admin")
        {
            Console.WriteLine("Unauthorized. Press Enter.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter Product ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Press Enter."); Console.ReadLine(); return;
        }

        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            Console.WriteLine("Product not found. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write($"New Qty (current {product.Qty}): ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
        {
            Console.WriteLine("Invalid qty. Press Enter."); Console.ReadLine(); return;
        }

        Console.Write($"New Price (current {product.Price}): ");
        if (!double.TryParse(Console.ReadLine(), out double price) || price < 0)
        {
            Console.WriteLine("Invalid price. Press Enter."); Console.ReadLine(); return;
        }

        product.Qty = qty;
        product.Price = price;
        Save();
        Logger.Log(user.Id, user.Role, "UPDATE", "Product", product.Id, "SUCCESS");
        Console.WriteLine("Product updated. Press Enter."); Console.ReadLine();
    }

    public void DeleteProduct(User user)
    {
        if (user.Role != "admin")
        {
            Console.WriteLine("Unauthorized. Press Enter.");
            Console.ReadLine();
            return;
        }

        Console.Write("Enter Product ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID. Press Enter."); Console.ReadLine(); return;
        }

        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            Console.WriteLine("Product not found. Press Enter."); Console.ReadLine(); return;
        }

        products.Remove(product);
        Save();
        Logger.Log(user.Id, user.Role, "DELETE", "Product", product.Id, "SUCCESS");
        Console.WriteLine("Product deleted. Press Enter."); Console.ReadLine();
    }
}
