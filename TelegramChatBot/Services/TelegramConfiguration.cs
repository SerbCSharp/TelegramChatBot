namespace TelegramChatBot.Services
{
    public class TelegramConfiguration
    {
        public const string Section = "TelegramConfiguration";
        public string BotToken { get; set; }
        public string UriFrontend { get; set; }
        public string UriBackend { get; set; }
    }
}
