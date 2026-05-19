using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data
{
    public class DonerDBContext : DbContext
    {
        DbSet<Users> Users { get; set; }
        DbSet<UserAddresses> UserAddresses { get; set; }
        DbSet<Categories> Categories { get; set; }
        DbSet<Products> Products { get; set; }
        DbSet<Orders> Orders { get; set; }
        DbSet<OrderDetails> OrderDetails { get; set; }
        DbSet<Payments> Payments { get; set; }
        DbSet<Deliveries> Deliveries { get; set; }
        DbSet<Suppliers> Suppliers { get; set; }
        DbSet<Ingredients> Ingredients { get; set; }
        DbSet<ProductIngredients> ProductIngredients { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(x =>
            {
                x.HasKey(u => u.UserId);

                x.Property(u => u.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
                x.HasIndex(u => u.UserName).IsUnique();

                x.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                x.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                x.HasIndex(u => u.Email).IsUnique();

                x.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                x.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                x.Property(u => u.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                x.Property(u => u.Role)
                    .IsRequired()
                    .HasConversion<string>();

                x.HasMany(u => u.UserAddresses)
                    .WithOne(ua => ua.User)
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<UserAddresses>(x =>
            {
                x.HasKey(ua => ua.UserAddressId);

                x.Property(ua => ua.AddressLine)
                    .IsRequired()
                    .HasMaxLength(200);

                x.Property(ua => ua.City)
                    .IsRequired()
                    .HasMaxLength(50);

                x.Property(ua => ua.Notes)
                    .HasMaxLength(200);

                x.Property(ua => ua.AddressType)
                    .IsRequired()
                    .HasConversion<string>();

                x.HasOne(ua => ua.User)
                    .WithMany(u => u.UserAddresses)
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Categories>(x =>
            {
                x.HasKey(c => c.CategoryId);

                x.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(50);
                x.HasIndex(c => c.Name).IsUnique();

                x.Property(c => c.Description)
                    .HasMaxLength(255);

                x.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Products>(x =>
            {
                x.HasKey(p => p.ProductId);

                x.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                x.HasIndex(p => p.Name).IsUnique();

                x.Property(p => p.Price)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.Property(p => p.IsAvailable)
                    .IsRequired();

                x.Property(p => p.ProductSize)
                    .HasConversion<string>();

                x.Property(p => p.MeatType)
                    .HasConversion<string>();

                x.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Orders>(x =>
            {
                x.HasKey(o => o.OrderId);

                x.Property(o => o.OrderDate)
                    .IsRequired();

                x.Property(o => o.OrderType)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(o => o.OrderStatus)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(o => o.TotalPrice)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<OrderDetails>(x =>
            {
                x.HasKey(od => od.OrderDetailsId);

                x.Property(od => od.Quantity)
                    .IsRequired();

                x.Property(od => od.UnitPrice)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.Property(od => od.Subtotal)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(od => od.Product)
                    .WithMany()
                    .HasForeignKey(od => od.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Payments>(x =>
            {
                x.HasKey(p => p.PaymentId);
                
                x.Property(p => p.PaymentMethod)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(p => p.PaymentStatus)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(p => p.AmountPaid)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.Property(p => p.PaymentDate)
                    .IsRequired();
                
                x.HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }
    }
}
