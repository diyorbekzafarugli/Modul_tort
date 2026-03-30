using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TelegramBot.Data;
using TelegramBot.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=bot_database.db"));

        services.AddHttpClient<ICurrencyService, CurrencyService>();
        services.AddScoped<BotHandler>();

        string botToken = Environment.GetEnvironmentVariable("MY_BOT_TOKEN")!;

        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(botToken));

        services.AddHostedService<BotHostedService>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

Console.WriteLine("Dastur ishlamoqda. To'xtatish uchun Ctrl+C bosing.");
await host.RunAsync();