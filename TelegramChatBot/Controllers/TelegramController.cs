using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Microsoft.Extensions.Options;
using TelegramChatBot.Services;
using TelegramChatBot.Infrastructure.Repositories;

namespace TelegramChatBot.Controllers
{
    [ApiController]
    [Route("/")]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramConfiguration _telegramConfiguration;
        private readonly ITelegramBotService _telegramBotService;
        private readonly RequestRepository _requestRepository;

        public TelegramController(IOptions<TelegramConfiguration> telegramConfiguration, ITelegramBotService telegramBotService, RequestRepository requestRepository)
        {
            _telegramConfiguration = telegramConfiguration.Value ?? throw new ArgumentNullException(nameof(telegramConfiguration));
            _telegramBotService = telegramBotService ?? throw new ArgumentNullException(nameof(telegramBotService));
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update)
        {
            var message = update.Message;
            if (message != null)
            {
                var resultToHtml = await _telegramBotService.ResponseAsync(message);
                var client = new TelegramBotClient(_telegramConfiguration.BotToken);
                await client.SendTextMessageAsync(message.Chat.Id, resultToHtml, parseMode: ParseMode.Html, null, true);
            }
            return Ok();
        }

        [Route("Requests")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(int skip, int take)
        {
            var request = await _requestRepository.GetRequestAsync(skip, take);
            return Ok(request);
        }
    }
}