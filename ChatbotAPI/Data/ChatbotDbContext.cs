using Microsoft.EntityFrameworkCore;
using ChatbotAPI.Models;

namespace ChatbotAPI.Data
{
    public class ChatbotDbContext : DbContext
    {
        public ChatbotDbContext(DbContextOptions<ChatbotDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WidgetSettings> WidgetSettings { get; set; }
        public DbSet<TrainingData> TrainingData { get; set; }
        public DbSet<ChatLog> ChatLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
    .HasOne(p => p.WidgetSettings)
    .WithOne(ws => ws.Project)
    .HasForeignKey<WidgetSettings>(ws => ws.ProjectId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.TrainingData)
                .WithOne(td => td.Project)
                .HasForeignKey<TrainingData>(td => td.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WidgetSettings>()
                .HasIndex(ws => ws.ProjectId)
                .IsUnique();

            modelBuilder.Entity<TrainingData>()
                .HasIndex(td => td.ProjectId)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
            // You can configure entity relationships and constraints here
        }
    }
}

