using Microsoft.EntityFrameworkCore;
using TelegramChatBot.Infrastructure.EntityConfigurations;
using TelegramChatBot.Model;

namespace TelegramChatBot.Infrastructure
{
    public class StatisticsContext : DbContext
    {
        public StatisticsContext(DbContextOptions<StatisticsContext> options) : base(options)
        {
        }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RequestEntityTypeConfiguration());
        }
    }
}
