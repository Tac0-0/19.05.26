using Doner.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Doner.Data
{
    public class DonerDBContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<UserAddresses> UserAddresses { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Deliveries> Deliveries { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        public DbSet<ProductIngredients> ProductIngredients { get; set; }
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
                x.HasDiscriminator<string>("UserType")
                    .HasValue<Customers>(nameof(Customers))
                    .HasValue<Employees>(nameof(Employees))
                    .HasValue<Admin>(nameof(Admin));

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

            modelBuilder.Entity<Employees>(x =>
            {
                x.Property(e => e.EmployeePosition)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(e => e.Salary)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.Property(e => e.HireDate)
                    .IsRequired()
                    .HasColumnType("datetime2");

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

            modelBuilder.Entity<Deliveries>(x =>
            {
                x.HasKey(d => d.DeliveryId);

                x.Property(d => d.DeliveryStatus)
                    .IsRequired()
                    .HasConversion<string>();

                x.Property(d => d.DeliveryFee)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                x.Property(d => d.EstimatedDeliveryTime)
                    .HasColumnType("datetime2");

                x.Property(d => d.DeliveredAt)
                    .HasColumnType("datetime2");

                x.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(d => d.DeliveryWorker)
                    .WithMany()
                    .HasForeignKey(d => d.DeliveryWorkerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                x.HasOne(d => d.Address)
                    .WithMany()
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Suppliers>(x =>
            {
                x.HasKey(s => s.SupplierId);

                x.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                x.Property(s => s.PhoneNumber)
                    .HasMaxLength(20);

                x.Property(s => s.Address)
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Ingredients>(x =>
            {
                x.HasKey(i => i.IngredientId);

                x.Property(i => i.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                x.Property(i => i.QuantityInStock)
                    .IsRequired()
                    .HasColumnType("decimal(10,3)");

                x.Property(i => i.Unit)
                    .IsRequired()
                    .HasConversion<string>();

                x.HasOne(i => i.Supplier)
                    .WithMany()
                    .HasForeignKey(i => i.SupplierId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductIngredients>(x =>
            {
                x.HasKey(pi => pi.ProductIngredientId);

                x.Property(pi => pi.RequiredQuantity)
                    .IsRequired()
                    .HasColumnType("decimal(10,3)");

                x.HasOne(pi => pi.Product)
                    .WithMany()
                    .HasForeignKey(pi => pi.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(pi => pi.Ingredient)
                    .WithMany()
                    .HasForeignKey(pi => pi.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }
    }
}
