namespace TelegramBot.Models;

public class User
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Login { get; set; }
    public string? PasswordHash { get; set; }
    public string? ApiToken { get; set; }

    // YAngi maydonlar:
    public string Role { get; set; } = "User"; // "User" yoki "Admin"
    public bool IsBlocked { get; set; } = false; // Bloklanganmi?
    public string Language { get; set; } = "UZ"; // Sozlamalar uchun

    public UserState State { get; set; } = UserState.None;
    public DateTime RegisteredAt { get; set; }

    public List<CurrencyLog> CurrencyLogs { get; set; } = new();
}