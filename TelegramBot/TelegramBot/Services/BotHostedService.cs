using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace TelegramBot.Services;

public class BotHostedService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BotHostedService> _logger;

    public BotHostedService(ITelegramBotClient botClient,
        IServiceProvider serviceProvider,
        ILogger<BotHostedService> logger)
    {
        _botClient = botClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🤖 Bot xizmati ishga tushmoqda...");

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: new ReceiverOptions(),
            cancellationToken: stoppingToken
        );

        _logger.LogInformation("✅ Bot xabarlarni kutmoqda!");
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<BotHandler>();
            await handler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient,
        Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var handler = scope.ServiceProvider.GetRequiredService<BotHandler>();
            await handler.HandleErrorAsync(botClient, exception, source, cancellationToken);
        }
    }
}