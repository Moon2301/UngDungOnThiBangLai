using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using BCrypt.Net;
using UngDungOnThiBangLai.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LicenseCategory> LicenseCategories { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionTopic> QuestionTopics { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<TrafficSign> TrafficSigns { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Cấu hình bảng LicenseCategory
        modelBuilder.Entity<LicenseCategory>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        });

        // 2. Cấu hình bảng Question
        modelBuilder.Entity<Question>(entity => {
            entity.HasKey(e => e.Id);

            // Quan hệ với LicenseCategory (N - 1)
            entity.HasOne(q => q.LicenseCategory)
                  .WithMany(c => c.Questions)
                  .HasForeignKey(q => q.LicenseCategoryId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với TrafficSign (N - 1) - Có thể null
            entity.HasOne(q => q.TrafficSign)
                  .WithMany(t => t.Questions)
                  .HasForeignKey(q => q.TrafficSignId)
                  .OnDelete(DeleteBehavior.SetNull); // Nếu xóa biển báo, câu hỏi vẫn còn nhưng mất link ảnh biển báo
        });

        // 3. Cấu hình bảng Answer
        modelBuilder.Entity<Answer>(entity => {
            entity.HasKey(e => e.Id);

            // Quan hệ với Question (N - 1)
            entity.HasOne(a => a.Question)
                  .WithMany(q => q.Answers)
                  .HasForeignKey(a => a.QuestionId)
                  .OnDelete(DeleteBehavior.Cascade); // Xóa câu hỏi thì xóa luôn đáp án

            // Quan hệ với TrafficSign (N - 1) - Có thể null
            entity.HasOne(a => a.TrafficSign)
                  .WithMany(t => t.Answers)
                  .HasForeignKey(a => a.TrafficSignId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // 4. Cấu hình bảng TrafficSign
        modelBuilder.Entity<TrafficSign>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        // 5. Cấu hình bảng User
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Question>()
        .HasOne(q => q.Topic)
        .WithMany(t => t.Questions)
        .HasForeignKey(q => q.QuestionTopicId)
        .OnDelete(DeleteBehavior.Restrict);

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