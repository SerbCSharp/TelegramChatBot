using Telegram.Bot.Types;

namespace TelegramChatBot.Services
{
    public interface ITelegramBotService
    {
        Task<string> ResponseAsync(Message message);
    }
}
