using Hoshiko.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hoshiko.Data.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Stage> Stages => Set<Stage>();
    public DbSet<StageSection> StageSections => Set<StageSection>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Stage>()
            .HasMany(s => s.Sections)
            .WithOne(s => s.Stage)
            .HasForeignKey(s => s.StageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StageSection>()
            .Property(s => s.Text)
            .IsRequired()
            .HasMaxLength(1000);

        modelBuilder.Entity<Stage>()
            .Property(s => s.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Stage>().HasData(
            new Stage
            {
                Id = 1,
                Title = "مرحله آزمایشی",
                Description = "این یک مرحله‌ی نمونه است برای تست سیستم."
            }
        );

        modelBuilder.Entity<StageSection>().HasData(
            new StageSection
            {
                Id = 1,
                StageId = 1,
                Type = SectionType.Content,
                Text = "در این بخش یاد می‌گیریم چطور مراحل بازی کار می‌کنند."
            },
            new StageSection
            {
                Id = 2,
                StageId = 1,
                Type = SectionType.Content,
                Text = "هر مرحله دارای چند توضیح و چند سوال است."
            },
            new StageSection
            {
                Id = 3,
                StageId = 1,
                Type = SectionType.Question,
                Text = "چند بخش در هر مرحله وجود دارد؟",
                OptionA = "دو بخش",
                OptionB = "چهار بخش",
                OptionC = "یک بخش",
                OptionD = "پنج بخش",
                CorrectAnswer = "B"
            },
            new StageSection
            {
                Id = 4,
                StageId = 1,
                Type = SectionType.Question,
                Text = "اگر پاسخ اشتباه بدهی چه می‌شود؟",
                OptionA = "به مرحله بعد می‌روی",
                OptionB = "بازی ریست می‌شود",
                OptionC = "پاسخ خط می‌خورد و ادامه می‌دهی",
                OptionD = "هیچ‌چیز",
                CorrectAnswer = "C"
            }
        );


        base.OnModelCreating(modelBuilder);
    }
}