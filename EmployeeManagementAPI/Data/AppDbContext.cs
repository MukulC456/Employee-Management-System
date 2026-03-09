using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasDefaultValue("Active");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Role).HasDefaultValue("User");
            });

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Arjun Sharma",
                    Email = "arjun@company.com",
                    Department = "Engineering",
                    Role = "Senior Developer",
                    Salary = 95000,
                    JoinDate = new DateTime(2021, 3, 15),
                    Status = "Active"
                },
                new Employee
                {
                    Id = 2,
                    Name = "Priya Mehta",
                    Email = "priya@company.com",
                    Department = "HR",
                    Role = "HR Manager",
                    Salary = 72000,
                    JoinDate = new DateTime(2020, 7, 1),
                    Status = "Active"
                },
                new Employee
                {
                    Id = 3,
                    Name = "Rohan Verma",
                    Email = "rohan@company.com",
                    Department = "Finance",
                    Role = "Financial Analyst",
                    Salary = 68000,
                    JoinDate = new DateTime(2022, 1, 10),
                    Status = "Active"
                }
            );
        }
    }
}