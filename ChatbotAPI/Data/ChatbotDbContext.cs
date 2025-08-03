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
        public DbSet<HelpfulLink> HelpfulLinks { get; set; }
        public DbSet<PdfFile> PdfFiles { get; set; }
        public DbSet<ChatLog> ChatLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // You can configure entity relationships and constraints here
        }
    }
}

