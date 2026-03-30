namespace TelegramBot.Models;

public class User
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }
    public string? ApiToken { get; set; }

    public string Role { get; set; } = "User"; 
    public bool IsBlocked { get; set; } = false;
    public string Language { get; set; } = "UZ";
    public bool WaitingForCurrencyInput { get; set; } = false;
    public UserState State { get; set; } = UserState.None;
    public DateTime RegisteredAt { get; set; }

    public List<CurrencyLog> CurrencyLogs { get; set; } = new();
}