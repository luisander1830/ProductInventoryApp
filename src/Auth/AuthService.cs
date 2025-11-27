using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SecureInventory.Logging;

namespace SecureInventory.Auth;

public class AuthService
{
    private List<User> users = new List<User>();
    private string filePath = "data/users.json";

    public AuthService()
    {
        LoadUsers();
    }

    private void LoadUsers()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }
    }

    private void SaveUsers()
    {
        File.WriteAllText(filePath, JsonConvert.SerializeObject(users, Formatting.Indented));
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public void Register()
    {
        Console.Write("Username (3-20 chars): ");
        string username = Console.ReadLine() ?? "";
        if (username.Length < 3 || username.Length > 20)
        {
            Console.WriteLine("Invalid length. Press Enter.");
            Console.ReadLine();
            return;
        }

        if (users.Any(u => u.Username == username))
        {
            Console.WriteLine("Username exists. Press Enter.");
            Console.ReadLine();
            return;
        }

        Console.Write("Password (min 6 chars): ");
        string password = Console.ReadLine() ?? "";
        if (password.Length < 6)
        {
            Console.WriteLine("Password too short. Press Enter.");
            Console.ReadLine();
            return;
        }

        var user = new User
        {
            Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1,
            Username = username,
            PasswordHash = HashPassword(password),
            Role = users.Count == 0 ? "admin" : "user",
            CreatedAt = DateTime.Now
        };

        users.Add(user);
        SaveUsers();
        Logger.Log(user.Id, user.Role, "REGISTER", "User", user.Id, "SUCCESS");
        Console.WriteLine($"User {username} registered. Press Enter.");
        Console.ReadLine();
    }

    public User? Login()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";
        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        var hash = HashPassword(password);
        var user = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);

        if (user == null)
        {
            Console.WriteLine("Invalid credentials. Press Enter.");
            Console.ReadLine();
            return null;
        }

        Logger.Log(user.Id, user.Role, "LOGIN", "User", user.Id, "SUCCESS");
        return user;
    }
}
