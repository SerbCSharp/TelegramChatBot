using DiagnosticWebAPIServer.Model;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot.Services
{
    public class MedicalDirectoryBotService : ITelegramBotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TelegramConfiguration _telegramConfiguration;

        public MedicalDirectoryBotService(IOptions<TelegramConfiguration> telegramConfiguration, IHttpClientFactory httpClientFactory)
        {
            _telegramConfiguration = telegramConfiguration.Value ?? throw new ArgumentNullException(nameof(telegramConfiguration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string> ResponseAsync(Message message)
        {
            var resultToHtml = "";
            if (message.Type == MessageType.Text)
            {
                if (message.Text == "/start")
                {
                }

                else if (message.Text != null)
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    var diseases = new List<DiseaseDto>();

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
