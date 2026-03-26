namespace TelegramBot.Models;

public class CurrencyLog
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; } = null!;
    public string CurrencyCode { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int ViewsCount { get; set; }
}