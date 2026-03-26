namespace TelegramBot.Services;

public interface ICurrencyService
{
    Task<string?> GetRateAsync(string currencyCode);
}