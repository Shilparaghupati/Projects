using CanteenWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenWeb.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        CanteenDbContext context)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("DATABASE INITIALIZATION");
        Console.WriteLine("--------------------------------");

        // =====================================================
        // 1. CREATE DATABASE
        // =====================================================

        Console.WriteLine("Ensuring database exists...");

        await context.Database.EnsureCreatedAsync();

        Console.WriteLine("Database exists.");


        // =====================================================
        // 2. CREATE / UPDATE ADMIN
        // =====================================================

        const string adminEmail = "admin@canteen.com";
        const string adminPassword = "Admin@123";

        Console.WriteLine("Checking admin account...");

        var admin = await context.Admins
            .FirstOrDefaultAsync(a =>
                a.Email == adminEmail);


        if (admin == null)
        {
            // -------------------------------------------------
            // CREATE ADMIN
            // -------------------------------------------------

            Console.WriteLine(
                "Admin does not exist. Creating admin...");

            admin = new Admin
            {
                Name = "Canteen Admin",
                Email = adminEmail,
                Password = adminPassword
            };

            context.Admins.Add(admin);

            await context.SaveChangesAsync();

            Console.WriteLine(
                "Admin created successfully.");
        }
        else
        {
            // -------------------------------------------------
            // UPDATE ADMIN
            // -------------------------------------------------

            Console.WriteLine(
                "Admin already exists. Updating admin...");

            admin.Name = "Canteen Admin";
            admin.Email = adminEmail;
            admin.Password = adminPassword;

            await context.SaveChangesAsync();

            Console.WriteLine(
                "Admin updated successfully.");
        }


        // =====================================================
        // 3. SHOW ADMIN LOGIN DETAILS
        // =====================================================

        Console.WriteLine("--------------------------------");
        Console.WriteLine("ADMIN LOGIN DETAILS");
        Console.WriteLine("--------------------------------");

        Console.WriteLine(
            $"Admin ID  : {admin.AdminId}");

        Console.WriteLine(
            $"Name      : {admin.Name}");

        Console.WriteLine(
            $"Email     : {admin.Email}");

        Console.WriteLine(
            $"Password  : {admin.Password}");

        Console.WriteLine("--------------------------------");


        // =====================================================
        // 4. CREATE SAMPLE FOOD ITEMS
        // =====================================================

        if (!await context.FoodItems.AnyAsync())
        {
            Console.WriteLine(
                "Creating sample food items...");

            var foodItems = new List<FoodItem>
            {
                new FoodItem
                {
                    Name = "Masala Dosa",
                    Category = "Breakfast",
                    Description =
                        "Crispy dosa with chutney and sambar.",
                    Price = 60,
                    ImageUrl = "",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                },

                new FoodItem
                {
                    Name = "Idli Vada",
                    Category = "Breakfast",
                    Description =
                        "Soft idli with crispy vada.",
                    Price = 50,
                    ImageUrl = "",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                },

                new FoodItem
                {
                    Name = "Veg Meals",
                    Category = "Lunch",
                    Description =
                        "Complete vegetarian meals.",
                    Price = 100,
                    ImageUrl = "",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                },

                new FoodItem
                {
                    Name = "Veg Sandwich",
                    Category = "Snacks",
                    Description =
                        "Fresh vegetable sandwich.",
                    Price = 50,
                    ImageUrl = "",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                },

                new FoodItem
                {
                    Name = "Coffee",
                    Category = "Drinks",
                    Description =
                        "Hot filter coffee.",
                    Price = 30,
                    ImageUrl = "",
                    IsAvailable = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.FoodItems.AddRange(foodItems);

            await context.SaveChangesAsync();

            Console.WriteLine(
                "Sample food items created.");
        }
        else
        {
            Console.WriteLine(
                "Food items already exist.");
        }


        // =====================================================
        // 5. CUSTOMER TEST PASSWORD
        // =====================================================

        var testCustomer =
            await context.Customers
                .FirstOrDefaultAsync(c =>
                    c.Email == "shilpa@gmail.com");

        if (testCustomer != null)
        {
            testCustomer.Password = "Shilpa@123";

            await context.SaveChangesAsync();

            Console.WriteLine(
                "Test customer password updated.");
        }


        // =====================================================
        // 6. COMPLETED
        // =====================================================

        Console.WriteLine("--------------------------------");
        Console.WriteLine("DATABASE SETUP COMPLETED");
        Console.WriteLine("--------------------------------");

        Console.WriteLine("ADMIN LOGIN:");
        Console.WriteLine(
            "Email    : admin@canteen.com");
        Console.WriteLine(
            "Password : Admin@123");

        Console.WriteLine("--------------------------------");
    }
}