using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UngDungOnThiBangLai.Models;
using BCrypt.Net;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<LicenseCategory> LicenseCategories { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình quan hệ 1-N giữa Question và Answer
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        // --- SEED DATA ---
        // Hạng bằng lái
        modelBuilder.Entity<LicenseCategory>().HasData(
            new LicenseCategory { Id = 1, Name = "A1", Description = "Xe máy dưới 175cc" },
            new LicenseCategory { Id = 2, Name = "B2", Description = "Ô tô con số sàn" }
        );

        // Tài khoản Admin mặc định 
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Email = "admin@onthi.com",
                Role = "Admin",
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );
    }


}