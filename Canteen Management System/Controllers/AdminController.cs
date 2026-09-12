using CanteenWeb.Data;
using CanteenWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CanteenWeb.Controllers;

public class AdminController : Controller
{
    private readonly CanteenDbContext _context;

    public AdminController(CanteenDbContext context)
    {
        _context = context;
    }


    // =====================================================
    // ADMIN LOGIN - GET
    // =====================================================

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    // =====================================================
    // ADMIN LOGIN - POST
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Login(
        string email,
        string password)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("ADMIN LOGIN BUTTON CLICKED");
        Console.WriteLine($"Email entered: {email}");
        Console.WriteLine("--------------------------------");


        // Check empty fields
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error =
                "Please enter email and password.";

            return View();
        }


        email = email.Trim();


        // =================================================
        // FIND ADMIN IN DATABASE
        // =================================================

        var admin = await _context.Admins
            .FirstOrDefaultAsync(a =>
                a.Email == email);


        if (admin == null)
        {
            Console.WriteLine("ADMIN NOT FOUND");

            ViewBag.Error =
                "Invalid admin email or password.";

            return View();
        }


        Console.WriteLine(
            $"ADMIN FOUND: {admin.Email}");

        Console.WriteLine(
            $"ADMIN ID: {admin.AdminId}");


        // =================================================
        // CHECK PASSWORD
        // =================================================

        if (admin.Password != password)
        {
            Console.WriteLine("PASSWORD INCORRECT");

            ViewBag.Error =
                "Invalid admin email or password.";

            return View();
        }


        Console.WriteLine("PASSWORD CORRECT");


        // =================================================
        // CREATE SESSION
        // =================================================

        HttpContext.Session.SetInt32(
            "AdminId",
            admin.AdminId);

        HttpContext.Session.SetString(
            "AdminName",
            admin.Name);


        // =================================================
        // CHECK SESSION
        // =================================================

        var sessionAdminId =
            HttpContext.Session.GetInt32(
                "AdminId");

        var sessionAdminName =
            HttpContext.Session.GetString(
                "AdminName");


        Console.WriteLine(
            $"SESSION ADMIN ID: {sessionAdminId}");

        Console.WriteLine(
            $"SESSION ADMIN NAME: {sessionAdminName}");


        if (sessionAdminId == null)
        {
            Console.WriteLine(
                "SESSION CREATION FAILED");

            ViewBag.Error =
                "Unable to create admin session.";

            return View();
        }


        // =================================================
        // LOGIN SUCCESS
        // =================================================

        Console.WriteLine(
            "ADMIN LOGIN SUCCESSFUL");

        Console.WriteLine(
            "REDIRECTING TO DASHBOARD");


        return RedirectToAction(
            nameof(Dashboard));
    }


    // =====================================================
    // ADMIN DASHBOARD
    // =====================================================

    [HttpGet]
    public IActionResult Dashboard()
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("ADMIN DASHBOARD REQUEST");
        Console.WriteLine("--------------------------------");


        // Get Admin ID from session
        var adminId =
            HttpContext.Session.GetInt32(
                "AdminId");


        // Get Admin Name
        var adminName =
            HttpContext.Session.GetString(
                "AdminName");


        Console.WriteLine(
            $"AdminId: {adminId}");

        Console.WriteLine(
            $"AdminName: {adminName}");


        // =================================================
        // ADMIN NOT LOGGED IN
        // =================================================

        if (adminId == null)
        {
            Console.WriteLine(
                "ADMIN SESSION NOT FOUND");

            return RedirectToAction(
                nameof(Login));
        }


        // =================================================
        // ADMIN LOGGED IN
        // =================================================

        ViewBag.AdminName =
            adminName ?? "Canteen Admin";


        Console.WriteLine(
            "ADMIN DASHBOARD OPENED");


        return View();
    }


    // =====================================================
    // MANAGE MENU
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> ManageMenu()
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        var foodItems =
            await _context.FoodItems
                .OrderBy(x => x.Category)
                .ThenBy(x => x.Name)
                .ToListAsync();


        return View(foodItems);
    }


    // =====================================================
    // ADD FOOD - GET
    // =====================================================

    [HttpGet]
    public IActionResult AddFood()
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }

        return View();
    }


    // =====================================================
    // ADD FOOD - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFood(
        FoodItem food)
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        if (!ModelState.IsValid)
        {
            return View(food);
        }


        food.CreatedAt =
            DateTime.UtcNow;


        _context.FoodItems.Add(food);

        await _context.SaveChangesAsync();


        TempData["Success"] =
            "Food item added successfully.";


        return RedirectToAction(
            nameof(ManageMenu));
    }


    // =====================================================
    // EDIT FOOD - GET
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> EditFood(
        int id)
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        var food =
            await _context.FoodItems
                .FirstOrDefaultAsync(
                    x => x.FoodId == id);


        if (food == null)
        {
            return NotFound();
        }


        return View(food);
    }


    // =====================================================
    // EDIT FOOD - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFood(
        int id,
        FoodItem food)
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        if (id != food.FoodId)
        {
            return NotFound();
        }


        if (!ModelState.IsValid)
        {
            return View(food);
        }


        var existingFood =
            await _context.FoodItems
                .FirstOrDefaultAsync(
                    x => x.FoodId == id);


        if (existingFood == null)
        {
            return NotFound();
        }


        existingFood.Name =
            food.Name;

        existingFood.Category =
            food.Category;

        existingFood.Description =
            food.Description;

        existingFood.Price =
            food.Price;

        existingFood.ImageUrl =
            food.ImageUrl;

        existingFood.IsAvailable =
            food.IsAvailable;


        await _context.SaveChangesAsync();


        TempData["Success"] =
            "Food item updated successfully.";


        return RedirectToAction(
            nameof(ManageMenu));
    }


    // =====================================================
    // DELETE FOOD
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFood(
        int id)
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        var food =
            await _context.FoodItems
                .FirstOrDefaultAsync(
                    x => x.FoodId == id);


        if (food == null)
        {
            return NotFound();
        }


        _context.FoodItems.Remove(food);

        await _context.SaveChangesAsync();


        TempData["Success"] =
            "Food item deleted successfully.";


        return RedirectToAction(
            nameof(ManageMenu));
    }


    // =====================================================
    // CUSTOMER ORDER HISTORY
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        var orders =
            await _context.Orders

                .Include(x => x.Customer)

                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.FoodItem)

                .Include(x => x.Payment)

                .OrderByDescending(
                    x => x.OrderDate)

                .ToListAsync();


        return View(orders);
    }


    // =====================================================
    // UPDATE CUSTOMER DELIVERY STATUS
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult>
        UpdateOrderStatus(
            int id,
            string status)
    {
        if (!IsAdminLoggedIn())
        {
            return RedirectToAction(
                nameof(Login));
        }


        var order =
            await _context.Orders
                .FirstOrDefaultAsync(
                    x => x.OrderId == id);


        if (order == null)
        {
            return NotFound();
        }


        // Allowed statuses
        var validStatuses = new[]
        {
            "Pending",
            "Preparing",
            "Ready",
            "Out for Delivery",
            "Delivered",
            "Cancelled"
        };


        if (!validStatuses.Contains(status))
        {
            TempData["Error"] =
                "Invalid delivery status.";

            return RedirectToAction(
                nameof(Orders));
        }


        order.Status = status;


        await _context.SaveChangesAsync();


        TempData["Success"] =
            $"Order #{id} status updated successfully.";


        return RedirectToAction(
            nameof(Orders));
    }


    // =====================================================
    // LOGOUT
    // =====================================================

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(
            nameof(Login));
    }


    // =====================================================
    // CHECK ADMIN SESSION
    // =====================================================

    private bool IsAdminLoggedIn()
    {
        return
            HttpContext.Session
                .GetInt32("AdminId") != null;
    }
}