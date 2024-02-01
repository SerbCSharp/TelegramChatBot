using DiagnosticWebAPIServer.Model;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramChatBot.Infrastructure.Repositories;
using TelegramChatBot.Model;

namespace TelegramChatBot.Services
{
    public class MedicalDirectoryBotService : ITelegramBotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramConfiguration _telegramConfiguration;
        private readonly RequestRepository _requestRepository;

        public MedicalDirectoryBotService(IOptions<TelegramConfiguration> telegramConfiguration, IHttpClientFactory httpClientFactory, RequestRepository requestRepository)
        {
            _telegramConfiguration = telegramConfiguration.Value ?? throw new ArgumentNullException(nameof(telegramConfiguration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
        }

        public async Task<string> ResponseAsync(Message message)
        {
            var resultToHtml = "";
            if (message.Type == MessageType.Text)
            {
                if (message.Text == "/start")
                {
                    await _requestRepository.CreateRequestAsync(new Request { ChatId = message.Chat.Id, Text = message.Text });
                }

                else if (message.Text != null)
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var diseases = new List<DiseaseDto>();

                    await _requestRepository.CreateRequestAsync(new Request { ChatId = message.Chat.Id, Text = message.Text });
                    var httpResponseMessage = await httpClient.GetAsync($"{_telegramConfiguration.UriBackend}Diagnostic/SearchForDiseasesBySymptom?message={message.Text}");
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        diseases = await httpResponseMessage.Content.ReadFromJsonAsync<List<DiseaseDto>>();

                    }
                    if (diseases?.Count > 0)
                    {
                        foreach (var disease in diseases)
                        {
                            resultToHtml = resultToHtml + 
                                @$"<a href='{_telegramConfiguration.UriFrontend}{disease.NameDisease}'><b>{disease.NameDisease}</b></a> - {disease.Count}" + "\n";
                        }
                    }
                    else
                    {
                        resultToHtml = "Не найдено. Уточните запрос";
                    }
                }
            }
            return resultToHtml;
        }
    }
}
