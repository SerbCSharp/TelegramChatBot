using Microsoft.EntityFrameworkCore;
using TelegramChatBot.Infrastructure;
using TelegramChatBot.Infrastructure.Repositories;
using TelegramChatBot.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient(); 
builder.Services.AddControllers();
builder.Services.AddDbContext<StatisticsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StatisticsDB")));
builder.Services.AddTransient<RequestRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var telegramConfiguration = builder.Configuration.GetSection(TelegramConfiguration.Section);
builder.Services.Configure<TelegramConfiguration>(telegramConfiguration);
builder.Services.AddTransient<ITelegramBotService, MedicalDirectoryBotService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
