using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramChatBot.Model;

namespace TelegramChatBot.Infrastructure.EntityConfigurations
{
    public class RequestEntityTypeConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");

            builder.Property(ci => ci.Id)
                .UseHiLo("request_hilo")
                .IsRequired();

            builder.Property(ci => ci.ChatId)
                .IsRequired(true);

            builder.Property(ci => ci.Text)
                .IsRequired(false);
        }
    }
}
