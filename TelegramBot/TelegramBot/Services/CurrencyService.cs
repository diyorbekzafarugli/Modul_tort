using System.Text.Json;
using TelegramBot.Models;

namespace TelegramBot.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetRateAsync(string currencyCode)
    {
        string url = "https://cbu.uz/ru/arkhiv-kursov-valyut/json/";
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var rates = JsonSerializer.Deserialize<List<CurrencyRate>>(json);

            var targetRate = rates?.FirstOrDefault(r =>
                r.Code.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));

            return targetRate?.Rate;
        }
        catch (Exception)
        {
            return null;
        }
    }
}