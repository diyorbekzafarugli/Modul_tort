using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Data;
using TelegramBot.Models;

namespace TelegramBot.Services;

public class BotHandler : IUpdateHandler
{
    private readonly AppDbContext _db;
    private readonly ICurrencyService _currencyService;

    // Asosiy menyu tugmalari
    private readonly ReplyKeyboardMarkup _mainMenu = new(new[]
    {
        new KeyboardButton[] { "💱 Valyuta so'rash", "📊 Statistika" },
        new KeyboardButton[] { "👤 Profil", "⚙️ Sozlamalar" }
    })
    { ResizeKeyboard = true };

    public BotHandler(AppDbContext db, ICurrencyService currencyService)
    {
        _db = db;
        _currencyService = currencyService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text) return;

        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim() ?? string.Empty;
        var tgUser = update.Message.From;

        if (tgUser == null) return;

        // 1. Foydalanuvchini topish yoki yaratish
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == tgUser.Id, cancellationToken);
        if (user == null)
        {
            user = new Models.User
            { 
               Id = tgUser.Id,
               Username = tgUser.Username,
               RegisteredAt = DateTime.UtcNow,
               State = UserState.WaitingForName
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
            await botClient.SendMessage(chatId, "Ilovamizga xush kelibsiz! Ism-sharifingizni kiriting:", cancellationToken: cancellationToken);
            return;
        }

        // 2. Bloklanganlarni tekshirish
        if (user.IsBlocked)
        {
            await botClient.SendMessage(chatId, "🚫 Sizning profilingiz bloklangan. Adminga murojaat qiling.", cancellationToken: cancellationToken);
            return;
        }

        // 3. Ro'yxatdan o'tish jarayoni (FSM)
        if (user.State != UserState.None)
        {
            await HandleRegistrationAsync(botClient, chatId, user, text, cancellationToken);
            return;
        }

        // 4. Admin Panel Buyruqlari
        if (user.Role == "Admin" && text.StartsWith("/"))
        {
            await HandleAdminCommandsAsync(botClient, chatId, text, cancellationToken);
            return;
        }

        // 5. Asosiy Ilova Menyusi
        switch (text)
        {
            case "/start":
                await botClient.SendMessage(chatId, "Asosiy menyu:", replyMarkup: _mainMenu, cancellationToken: cancellationToken);
                break;
            case "💱 Valyuta so'rash":
                await botClient.SendMessage(chatId, "Valyuta kodini yozing (masalan: USD, EUR, RUB):", cancellationToken: cancellationToken);
                break;
            case "👤 Profil":
                await ShowProfileAsync(botClient, chatId, user, cancellationToken);
                break;
            case "📊 Statistika":
                await ShowStatisticsAsync(botClient, chatId, user, cancellationToken);
                break;
            case "⚙️ Sozlamalar":
                await botClient.SendMessage(chatId, "⚙️ Sozlamalar bo'limi tez orada ishga tushadi!", replyMarkup: _mainMenu, cancellationToken: cancellationToken);
                break;
            case "/secretadmin": // O'zingizni admin qilish uchun maxfiy kod
                user.Role = "Admin";
                await _db.SaveChangesAsync(cancellationToken);
                await botClient.SendMessage(chatId, "👑 Tabriklaymiz, siz Admin bo'ldingiz!\nBuyruqlar: /users, /block [id], /unblock [id]", cancellationToken: cancellationToken);
                break;
            default:
                // Agar tugma bosilmagan bo'lsa, demak u valyuta kodini yozgan (masalan "USD")
                if (text.Length == 3)
                    await ProcessCurrencyAsync(botClient, chatId, user, text.ToUpper(), cancellationToken);
                else
                    await botClient.SendMessage(chatId, "Kechirasiz, buyruqni tushunmadim. Menyudan foydalaning.", replyMarkup: _mainMenu, cancellationToken: cancellationToken);
                break;
        }
    }

    // --- YORDAMCHI METODLAR ---

    private async Task HandleRegistrationAsync(ITelegramBotClient botClient, long chatId, TelegramBot.Models.User user, string text, CancellationToken ct)
    {
        if (user.State == UserState.WaitingForName)
        {
            user.FullName = text; user.State = UserState.WaitingForLogin;
            await botClient.SendMessage(chatId, "Endi login o'ylab toping:", cancellationToken: ct);
        }
        else if (user.State == UserState.WaitingForLogin)
        {
            user.Login = text; user.State = UserState.WaitingForPassword;
            await botClient.SendMessage(chatId, "Maxfiy parol kiriting:", cancellationToken: ct);
        }
        else if (user.State == UserState.WaitingForPassword)
        {
            user.PasswordHash = text; user.ApiToken = Guid.NewGuid().ToString("N"); user.State = UserState.None;
            await botClient.SendMessage(chatId, $"✅ Ro'yxatdan o'tdingiz!\nTokeningiz: <code>{user.ApiToken}</code>", parseMode: ParseMode.Html, replyMarkup: _mainMenu, cancellationToken: ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    private async Task ShowProfileAsync(ITelegramBotClient botClient, long chatId, TelegramBot.Models.User user, CancellationToken ct)
    {
        string profile = $"👤 **Sizning Profilingiz**\n\n" +
                         $"ID: {user.Id}\n" +
                         $"Ism: {user.FullName}\n" +
                         $"Login: @{user.Login}\n" +
                         $"Rol: {user.Role}\n" +
                         $"Ro'yxatdan o'tgan: {user.RegisteredAt:dd.MM.yyyy}";
        await botClient.SendMessage(chatId, profile, replyMarkup: _mainMenu, cancellationToken: ct);
    }

    private async Task ShowStatisticsAsync(ITelegramBotClient botClient, long chatId, TelegramBot.Models.User user, CancellationToken ct)
    {
        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);

        // Oxirgi 7 kundagi jami statistika
        var stats = await _db.CurrencyLogs
            .Where(l => l.UserId == user.Id && l.Date >= sevenDaysAgo)
            .GroupBy(l => l.CurrencyCode)
            .Select(g => new { Currency = g.Key, Total = g.Sum(x => x.ViewsCount) })
            .ToListAsync(ct);

        string response = "📊 **Oxirgi 7 kunlik statistika:**\n\n";
        if (stats.Count == 0) response += "Siz hali valyuta so'ramagansiz.";

        foreach (var item in stats)
            response += $"🔸 {item.Currency}: {item.Total} marta so'ralgan\n";

        await botClient.SendMessage(chatId, response, replyMarkup: _mainMenu, cancellationToken: ct);
    }

    private async Task ProcessCurrencyAsync(ITelegramBotClient botClient, long chatId, TelegramBot.Models.User user, string code, CancellationToken ct)
    {
        var rate = await _currencyService.GetRateAsync(code);
        if (rate == null)
        {
            await botClient.SendMessage(chatId, $"'{code}' valyutasi topilmadi.", cancellationToken: ct);
            return;
        }

        DateTime today = DateTime.UtcNow.Date;
        var log = await _db.CurrencyLogs.FirstOrDefaultAsync(l => l.UserId == user.Id && l.CurrencyCode == code && l.Date == today, ct);

        if (log == null)
        {
            log = new CurrencyLog { UserId = user.Id, CurrencyCode = code, Date = today, ViewsCount = 1 };
            _db.CurrencyLogs.Add(log);
        }
        else log.ViewsCount++;

        await _db.SaveChangesAsync(ct);
        await botClient.SendMessage(chatId, $"💰 1 {code} = {rate} so'm.\n(Bugun {log.ViewsCount} marta ko'rdingiz)", replyMarkup: _mainMenu, cancellationToken: ct);
    }

    private async Task HandleAdminCommandsAsync(ITelegramBotClient botClient, long chatId, string text, CancellationToken ct)
    {
        var parts = text.Split(' ');
        var command = parts[0].ToLower();

        if (command == "/users")
        {
            var users = await _db.Users.ToListAsync(ct);
            string res = "👥 **Foydalanuvchilar ro'yxati:**\n\n";
            foreach (var u in users) res += $"- ID:{u.Id} | {u.FullName} | Blok: {u.IsBlocked}\n";
            await botClient.SendMessage(chatId, res, cancellationToken: ct);
        }
        else if (command == "/block" && parts.Length > 1 && long.TryParse(parts[1], out long blockId))
        {
            var u = await _db.Users.FindAsync(new object[] { blockId }, ct); // To'g'rilangan joy
            if (u != null) { u.IsBlocked = true; await _db.SaveChangesAsync(ct); await botClient.SendMessage(chatId, $"Foydalanuvchi {blockId} bloklandi.", cancellationToken: ct); }
        }
        else if (command == "/unblock" && parts.Length > 1 && long.TryParse(parts[1], out long unblockId))
        {
            var u = await _db.Users.FindAsync(new object[] { unblockId }, ct); // To'g'rilangan joy
            if (u != null) { u.IsBlocked = false; await _db.SaveChangesAsync(ct); await botClient.SendMessage(chatId, $"Foydalanuvchi {unblockId} blokdan chiqarildi.", cancellationToken: ct); }
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) => Task.CompletedTask;
}