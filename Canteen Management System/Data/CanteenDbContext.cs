using CanteenWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CanteenWeb.Data;

public class CanteenDbContext : DbContext
{
    public CanteenDbContext(
        DbContextOptions<CanteenDbContext> options)
        : base(options)
    {
    }

    // =====================================================
    // TABLES
    // =====================================================

    public DbSet<Admin> Admins { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<FoodItem> FoodItems { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Payment> Payments { get; set; }

    

    // =====================================================
    // RELATIONSHIPS
    // =====================================================

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // =================================================
        // CUSTOMER -> ORDERS
        // One Customer can have many Orders
        // =================================================

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


        // =================================================
        // ORDER -> ORDER ITEMS
        // One Order can have many OrderItems
        // =================================================

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        // =================================================
        // FOOD ITEM -> ORDER ITEMS
        // One FoodItem can appear in many OrderItems
        // =================================================

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.FoodItem)
            .WithMany()
            .HasForeignKey(oi => oi.FoodId)
            .OnDelete(DeleteBehavior.Restrict);


        // =================================================
        // ORDER -> PAYMENT
        // One Order has one Payment
        // =================================================

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);


        // =================================================
        // DECIMAL PRECISION
        // =================================================

        modelBuilder.Entity<FoodItem>()
            .Property(f => f.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasPrecision(10, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(10, 2);
    }
}