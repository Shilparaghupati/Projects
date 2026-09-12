using CanteenWeb.Data;
using CanteenWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CanteenWeb.Controllers;

public class CustomerController : Controller
{
    private readonly CanteenDbContext _context;

    public CustomerController(CanteenDbContext context)
    {
        _context = context;
    }

    // =====================================================
    // CUSTOMER OPTIONS
    // =====================================================

    [HttpGet]
    public IActionResult Customer()
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        ViewBag.CustomerName =
            HttpContext.Session.GetString("CustomerName");

        return View();
    }


    // =====================================================
    // REGISTER - GET
    // =====================================================

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    // =====================================================
    // REGISTER - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        Customer customer,
        string confirmPassword)
    {
        // Password confirmation
        if (customer.Password != confirmPassword)
        {
            ModelState.AddModelError(
                "Password",
                "Password and Confirm Password do not match.");
        }

        // Password validation
        if (!string.IsNullOrEmpty(customer.Password))
        {
            bool hasUppercase =
                customer.Password.Any(char.IsUpper);

            bool hasLowercase =
                customer.Password.Any(char.IsLower);

            bool hasNumber =
                customer.Password.Any(char.IsDigit);

            bool hasSpecial =
                customer.Password.Any(
                    ch => !char.IsLetterOrDigit(ch));

            if (customer.Password.Length < 8)
            {
                ModelState.AddModelError(
                    "Password",
                    "Password must contain at least 8 characters.");
            }

            if (!hasUppercase)
            {
                ModelState.AddModelError(
                    "Password",
                    "Password must contain an uppercase letter.");
            }

            if (!hasLowercase)
            {
                ModelState.AddModelError(
                    "Password",
                    "Password must contain a lowercase letter.");
            }

            if (!hasNumber)
            {
                ModelState.AddModelError(
                    "Password",
                    "Password must contain a number.");
            }

            if (!hasSpecial)
            {
                ModelState.AddModelError(
                    "Password",
                    "Password must contain a special character.");
            }
        }

        // Phone validation
        if (!string.IsNullOrWhiteSpace(customer.Phone))
        {
            if (customer.Phone.Length != 10 ||
                !customer.Phone.All(char.IsDigit) ||
                customer.Phone[0] < '6' ||
                customer.Phone[0] > '9')
            {
                ModelState.AddModelError(
                    "Phone",
                    "Phone number must contain 10 digits and start with 6-9.");
            }
        }

        // Email validation
        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            if (!customer.Email.EndsWith(
                    "@gmail.com",
                    StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    "Email",
                    "Please use a Gmail address.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        // Check duplicate email
        var existingCustomer =
            await _context.Customers
                .FirstOrDefaultAsync(c =>
                    c.Email.ToLower() ==
                    customer.Email.ToLower());

        if (existingCustomer != null)
        {
            ModelState.AddModelError(
                "Email",
                "Customer with this email already exists.");

            return View(customer);
        }

        // Save customer
        customer.CreatedAt = DateTime.UtcNow;

        _context.Customers.Add(customer);

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Registration successful. Please login.";

        return RedirectToAction(nameof(Login));
    }


    // =====================================================
    // LOGIN - GET
    // =====================================================

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    // =====================================================
    // LOGIN - POST
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error =
                "Please enter email and password.";

            return View();
        }

        var customer =
            await _context.Customers
                .FirstOrDefaultAsync(c =>
                    c.Email.ToLower() ==
                    email.ToLower());

        if (customer == null)
        {
            ViewBag.Error =
                "Customer not found. Please sign up first.";

            return View();
        }

        if (customer.Password != password)
        {
            ViewBag.Error =
                "Invalid email or password.";

            return View();
        }

        // Store customer session
        HttpContext.Session.SetInt32(
            "CustomerId",
            customer.CustomerId);

        HttpContext.Session.SetString(
            "CustomerName",
            customer.Name);

        // Start a fresh cart
        SaveCart(new List<CartSessionItem>());

        // Successful login -> Menu
        return RedirectToAction(nameof(Menu));
    }


    // =====================================================
    // MENU + SEARCH
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> Menu(string? search)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var query = _context.FoodItems
            .Where(x => x.IsAvailable)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(x =>
                x.Name.Contains(search) ||
                x.Category.Contains(search));
        }

        var foodItems = await query
            .OrderBy(x => x.Category)
            .ThenBy(x => x.Name)
            .ToListAsync();

        ViewBag.Search = search;

        return View(foodItems);
    }


    // =====================================================
    // ADD TO CART
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int foodId)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var food = await _context.FoodItems
            .FirstOrDefaultAsync(x =>
                x.FoodId == foodId &&
                x.IsAvailable);

        if (food == null)
        {
            TempData["Error"] =
                "Food item is not available.";

            return RedirectToAction(nameof(Menu));
        }

        var cart = GetCart();

        var existingItem =
            cart.FirstOrDefault(x =>
                x.FoodId == foodId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            cart.Add(new CartSessionItem
            {
                FoodId = food.FoodId,
                Name = food.Name,
                Category = food.Category,
                Description = food.Description,
                Price = food.Price,
                ImageUrl = food.ImageUrl,
                Quantity = 1
            });
        }

        SaveCart(cart);

        TempData["Success"] =
            $"{food.Name} added to cart.";

        return RedirectToAction(nameof(Menu));
    }


    // =====================================================
    // CART
    // =====================================================

    [HttpGet]
    public IActionResult Cart()
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        ViewBag.Total =
            cart.Sum(x => x.Price * x.Quantity);

        return View(cart);
    }


    // =====================================================
    // INCREASE QUANTITY
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult IncreaseQuantity(int foodId)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.FoodId == foodId);

        if (item != null)
        {
            item.Quantity++;
        }

        SaveCart(cart);

        return RedirectToAction(nameof(Cart));
    }


    // =====================================================
    // DECREASE QUANTITY
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DecreaseQuantity(int foodId)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.FoodId == foodId);

        if (item != null)
        {
            item.Quantity--;

            if (item.Quantity <= 0)
            {
                cart.Remove(item);
            }
        }

        SaveCart(cart);

        return RedirectToAction(nameof(Cart));
    }


    // =====================================================
    // REMOVE FROM CART
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int foodId)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        var item = cart.FirstOrDefault(
            x => x.FoodId == foodId);

        if (item != null)
        {
            cart.Remove(item);
        }

        SaveCart(cart);

        return RedirectToAction(nameof(Cart));
    }


    // =====================================================
    // CLEAR CART
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearCart()
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        SaveCart(new List<CartSessionItem>());

        TempData["Success"] =
            "Cart cleared successfully.";

        return RedirectToAction(nameof(Cart));
    }


    // =====================================================
    // CHECKOUT
    // =====================================================

    [HttpGet]
    public IActionResult Checkout()
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        if (cart.Count == 0)
        {
            TempData["Error"] =
                "Your cart is empty.";

            return RedirectToAction(nameof(Menu));
        }

        var totalAmount =
            cart.Sum(x => x.Price * x.Quantity);

        ViewBag.TotalAmount = totalAmount;

        return View(cart);
    }


    // =====================================================
    // PROCESS PAYMENT
    // =====================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessPayment(
        string paymentMethod)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var cart = GetCart();

        if (cart.Count == 0)
        {
            TempData["Error"] =
                "Your cart is empty.";

            return RedirectToAction(nameof(Menu));
        }

        if (paymentMethod != "Online" &&
            paymentMethod != "Cash")
        {
            TempData["Error"] =
                "Please select a valid payment method.";

            return RedirectToAction(nameof(Checkout));
        }

        var customerId =
            HttpContext.Session.GetInt32("CustomerId");

        if (customerId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var totalAmount =
            cart.Sum(x => x.Price * x.Quantity);

        // Create Order
        var order = new Order
        {
            CustomerId = customerId.Value,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = "Pending"
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        // Create Order Items
        foreach (var item in cart)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                FoodId = item.FoodId,
                Quantity = item.Quantity,
                Price = item.Price
            };

            _context.OrderItems.Add(orderItem);
        }

        // Create Payment
        var payment = new Payment
        {
            OrderId = order.OrderId,
            Amount = totalAmount,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Paid",
            PaymentDate = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        // Clear cart
        SaveCart(new List<CartSessionItem>());

        // Go to success page
        return RedirectToAction(
            nameof(PaymentSuccess),
            new
            {
                orderId = order.OrderId
            });
    }


    // =====================================================
    // PAYMENT SUCCESS
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> PaymentSuccess(
        int orderId)
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var customerId =
            HttpContext.Session.GetInt32("CustomerId");

        var order = await _context.Orders
            .Include(x => x.Customer)
            .Include(x => x.OrderItems)
                .ThenInclude(x => x.FoodItem)
            .Include(x => x.Payment)
            .FirstOrDefaultAsync(x =>
                x.OrderId == orderId &&
                x.CustomerId == customerId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }


    // =====================================================
    // CUSTOMER ORDER HISTORY
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> OrderHistory()
    {
        if (!IsCustomerLoggedIn())
        {
            return RedirectToAction(nameof(Login));
        }

        var customerId =
            HttpContext.Session.GetInt32("CustomerId");

        if (customerId == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var orders = await _context.Orders
            .Where(o =>
                o.CustomerId == customerId.Value)

            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)

            .Include(o => o.Payment)

            .OrderByDescending(o => o.OrderDate)

            .ToListAsync();

        return View(orders);
    }


    // =====================================================
    // LOGOUT
    // =====================================================

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("CustomerId");
        HttpContext.Session.Remove("CustomerName");
        HttpContext.Session.Remove("Cart");

        return RedirectToAction(nameof(Login));
    }


    // =====================================================
    // CHECK CUSTOMER LOGIN
    // =====================================================

    private bool IsCustomerLoggedIn()
    {
        return HttpContext.Session
            .GetInt32("CustomerId") != null;
    }


    // =====================================================
    // GET CART FROM SESSION
    // =====================================================

    private List<CartSessionItem> GetCart()
    {
        var cartJson =
            HttpContext.Session.GetString("Cart");

        if (string.IsNullOrEmpty(cartJson))
        {
            return new List<CartSessionItem>();
        }

        try
        {
            return JsonSerializer
                .Deserialize<List<CartSessionItem>>(
                    cartJson)
                ?? new List<CartSessionItem>();
        }
        catch
        {
            return new List<CartSessionItem>();
        }
    }


    // =====================================================
    // SAVE CART TO SESSION
    // =====================================================

    private void SaveCart(
        List<CartSessionItem> cart)
    {
        var cartJson =
            JsonSerializer.Serialize(cart);

        HttpContext.Session.SetString(
            "Cart",
            cartJson);
    }
}