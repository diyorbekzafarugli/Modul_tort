using System.Text.Json.Serialization;

namespace TelegramBot.Models;

public class CurrencyRate
{
    [JsonPropertyName("Ccy")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("Rate")]
    public string Rate { get; set; } = string.Empty;
}