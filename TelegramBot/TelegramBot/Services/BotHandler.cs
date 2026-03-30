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
    private static InlineKeyboardMarkup MainMenu => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("💱 Valyuta so'rash", "menu:currency"),
            InlineKeyboardButton.WithCallbackData("📊 Statistika",      "menu:stats")
        ],
        [
            InlineKeyboardButton.WithCallbackData("👤 Profil",    "menu:profile"),
            InlineKeyboardButton.WithCallbackData("⚙️ Sozlamalar", "menu:settings")
        ]
    ]);
    private static InlineKeyboardMarkup CurrencyMenu => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("🇺🇸 USD", "currency:USD"),
            InlineKeyboardButton.WithCallbackData("🇪🇺 EUR", "currency:EUR"),
            InlineKeyboardButton.WithCallbackData("🇷🇺 RUB", "currency:RUB")
        ],
        [
            InlineKeyboardButton.WithCallbackData("🇬🇧 GBP", "currency:GBP"),
            InlineKeyboardButton.WithCallbackData("🇯🇵 JPY", "currency:JPY"),
            InlineKeyboardButton.WithCallbackData("🇨🇳 CNY", "currency:CNY")
        ],
        [
            InlineKeyboardButton.WithCallbackData("✏️ O'zim kiritaman", "currency:manual"),
            InlineKeyboardButton.WithCallbackData("🔙 Orqaga",           "menu:back")
        ]
    ]);
    private static InlineKeyboardMarkup AdminMenu => new(
    [
        [
            InlineKeyboardButton.WithCallbackData("👥 Foydalanuvchilar", "admin:users"),
            InlineKeyboardButton.WithCallbackData("🔙 Orqaga",           "menu:back")
        ]
    ]);

    public BotHandler(AppDbContext db, ICurrencyService currencyService)
    {
        _db = db;
        _currencyService = currencyService;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            await HandleCallbackAsync(botClient, update.CallbackQuery, cancellationToken);
            return;
        }

        if (update.Type != UpdateType.Message ||
            update.Message?.Type != MessageType.Text) return;

        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim() ?? string.Empty;
        var tgUser = update.Message.From;
        if (tgUser == null) return;

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
            await botClient.SendMessage(chatId,
                "👋 Ilovamizga xush kelibsiz!\n\nIsm-sharifingizni kiriting:",
                cancellationToken: cancellationToken);
            return;
        }

        if (user.IsBlocked)
        {
            await botClient.SendMessage(chatId,
                "🚫 Sizning profilingiz bloklangan. Adminga murojaat qiling.",
                cancellationToken: cancellationToken);
            return;
        }

        if (user.State != UserState.None)
        {
            await HandleRegistrationAsync(botClient, chatId, user, text, cancellationToken);
            return;
        }

        if (user.State == UserState.None && user.WaitingForCurrencyInput)
        {
            user.WaitingForCurrencyInput = false;
            await _db.SaveChangesAsync(cancellationToken);
            if (text.Length >= 3 && text.Length <= 5)
                await ProcessCurrencyAsync(botClient, chatId, user, text.ToUpper(), cancellationToken);
            else
                await botClient.SendMessage(chatId,
                    "❌ Noto'g'ri format. Masalan: USD, EUR, RUB",
                    replyMarkup: MainMenu, cancellationToken: cancellationToken);
            return;
        }

        if (user.Role == "Admin" && text.StartsWith("/") && text != "/start")
        {
            await HandleAdminCommandsAsync(botClient, chatId, text, cancellationToken);
            return;
        }

        if (text == "/start")
        {
            await botClient.SendMessage(chatId,
                "🏠 <b>Asosiy menyu</b>\n\nQuyidagi bo'limlardan birini tanlang:",
                parseMode: ParseMode.Html,
                replyMarkup: MainMenu,
                cancellationToken: cancellationToken);
            return;
        }

        if (text == "/secretadmin")
        {
            user.Role = "Admin";
            await _db.SaveChangesAsync(cancellationToken);
            await botClient.SendMessage(chatId,
                "👑 <b>Tabriklaymiz, siz Admin bo'ldingiz!</b>",
                parseMode: ParseMode.Html,
                replyMarkup: MainMenu,
                cancellationToken: cancellationToken);
            return;
        }

        await botClient.SendMessage(chatId,
            "ℹ️ Menyudan foydalaning 👇",
            replyMarkup: MainMenu,
            cancellationToken: cancellationToken);
    }

    private async Task HandleCallbackAsync(ITelegramBotClient botClient,
        CallbackQuery callback, CancellationToken ct)
    {
        var chatId = callback.Message!.Chat.Id;
        var messageId = callback.Message.MessageId;
        var data = callback.Data ?? string.Empty;

        await botClient.AnswerCallbackQuery(callback.Id, cancellationToken: ct);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == callback.From.Id, ct);
        if (user == null || user.IsBlocked) return;

        switch (data)
        {
            case "menu:back":
                await EditOrSend(botClient, chatId, messageId,
                    "🏠 <b>Asosiy menyu</b>\n\nQuyidagi bo'limlardan birini tanlang:",
                    MainMenu, ct);
                break;

            case "menu:currency":
                await EditOrSend(botClient, chatId, messageId,
                    "💱 <b>Valyuta tanlang:</b>",
                    CurrencyMenu, ct);
                break;

            case "menu:stats":
                await ShowStatisticsAsync(botClient, chatId, messageId, user, ct);
                break;

            case "menu:profile":
                await ShowProfileAsync(botClient, chatId, messageId, user, ct);
                break;

            case "menu:settings":
                await EditOrSend(botClient, chatId, messageId,
                    "⚙️ <b>Sozlamalar</b>\n\nBu bo'lim tez orada ishga tushadi!",
                    new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("🔙 Orqaga", "menu:back")
                    ), ct);
                break;

            case string s when s.StartsWith("currency:"):
                {
                    var code = s["currency:".Length..];
                    if (code == "manual")
                    {
                        user.WaitingForCurrencyInput = true;
                        await _db.SaveChangesAsync(ct);
                        await EditOrSend(botClient, chatId, messageId,
                            "✏️ Valyuta kodini yozing (masalan: <code>GBP</code>, <code>JPY</code>):",
                            new InlineKeyboardMarkup(
                                InlineKeyboardButton.WithCallbackData("🔙 Bekor qilish", "menu:currency")
                            ), ct);
                    }
                    else
                    {
                        await ProcessCurrencyAsync(botClient, chatId, user, code, ct, messageId);
                    }
                    break;
                }

            case "admin:users" when user.Role == "Admin":
                await ShowAdminUsersAsync(botClient, chatId, messageId, ct);
                break;

            case string s when s.StartsWith("admin:block:") && user.Role == "Admin":
                {
                    var targetId = long.Parse(s["admin:block:".Length..]);
                    await ToggleBlockAsync(botClient, chatId, messageId, targetId, true, ct);
                    break;
                }
            case string s when s.StartsWith("admin:unblock:") && user.Role == "Admin":
                {
                    var targetId = long.Parse(s["admin:unblock:".Length..]);
                    await ToggleBlockAsync(botClient, chatId, messageId, targetId, false, ct);
                    break;
                }
        }
    }

    /// Xabarni edit qilishga harakat qiladi, bo'lmasa yangi yuboradi
    private static async Task EditOrSend(ITelegramBotClient bot, long chatId,
        int messageId, string text, InlineKeyboardMarkup keyboard, CancellationToken ct)
    {
        try
        {
            await bot.EditMessageText(chatId, messageId, text,
                parseMode: ParseMode.Html,
                replyMarkup: keyboard,
                cancellationToken: ct);
        }
        catch
        {
            await bot.SendMessage(chatId, text,
                parseMode: ParseMode.Html,
                replyMarkup: keyboard,
                cancellationToken: ct);
        }
    }

    private async Task HandleRegistrationAsync(ITelegramBotClient botClient,
        long chatId, Models.User user, string text, CancellationToken ct)
    {
        if (user.State == UserState.WaitingForName)
        {
            user.FullName = text;
            user.State = UserState.WaitingForLogin;
            await botClient.SendMessage(chatId, "📝 Endi login o'ylab toping:", cancellationToken: ct);
        }
        else if (user.State == UserState.WaitingForLogin)
        {
            user.Login = text;
            user.State = UserState.WaitingForPassword;
            await botClient.SendMessage(chatId, "🔐 Maxfiy parol kiriting:", cancellationToken: ct);
        }
        else if (user.State == UserState.WaitingForPassword)
        {
            user.PasswordHash = text;
            user.ApiToken = Guid.NewGuid().ToString("N");
            user.State = UserState.None;
            await botClient.SendMessage(chatId,
                $"✅ <b>Ro'yxatdan muvaffaqiyatli o'tdingiz!</b>\n\n" +
                $"🔑 Tokeningiz: <code>{user.ApiToken}</code>",
                parseMode: ParseMode.Html,
                replyMarkup: MainMenu,
                cancellationToken: ct);
        }
        await _db.SaveChangesAsync(ct);
    }

    private async Task ShowProfileAsync(ITelegramBotClient botClient,
        long chatId, int messageId, Models.User user, CancellationToken ct)
    {
        var keyboard = new InlineKeyboardMarkup(
        [
            [InlineKeyboardButton.WithCallbackData("🔙 Orqaga", "menu:back")]
        ]);

        string isAdmin = user.Role == "Admin"
            ? "\n\n⚙️ <a href='https://t.me'>Admin Panel</a>"
            : string.Empty;

        string text =
            $"👤 <b>Sizning Profilingiz</b>\n\n" +
            $"🆔 ID: <code>{user.Id}</code>\n" +
            $"📛 Ism: {user.FullName}\n" +
            $"🔖 Login: @{user.Login}\n" +
            $"🎖 Rol: {user.Role}\n" +
            $"📅 Ro'yxatdan o'tgan: {user.RegisteredAt:dd.MM.yyyy}" +
            isAdmin;

        await EditOrSend(botClient, chatId, messageId, text, keyboard, ct);
    }

    private async Task ShowStatisticsAsync(ITelegramBotClient botClient,
        long chatId, int messageId, Models.User user, CancellationToken ct)
    {
        var keyboard = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData("🔙 Orqaga", "menu:back")
        );

        var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-7);
        var stats = await _db.CurrencyLogs
            .Where(l => l.UserId == user.Id && l.Date >= sevenDaysAgo)
            .GroupBy(l => l.CurrencyCode)
            .Select(g => new { Currency = g.Key, Total = g.Sum(x => x.ViewsCount) })
            .ToListAsync(ct);

        string response = "📊 <b>Oxirgi 7 kunlik statistika:</b>\n\n";
        if (stats.Count == 0)
            response += "📭 Siz hali valyuta so'ramagansiz.";
        else
            foreach (var item in stats)
                response += $"🔸 <b>{item.Currency}</b>: {item.Total} marta so'ralgan\n";

        await EditOrSend(botClient, chatId, messageId, response, keyboard, ct);
    }

    private async Task ProcessCurrencyAsync(ITelegramBotClient botClient,
        long chatId, Models.User user, string code, CancellationToken ct,
        int? editMessageId = null)
    {
        var keyboard = new InlineKeyboardMarkup(
        [
            [
                InlineKeyboardButton.WithCallbackData("💱 Boshqa valyuta", "menu:currency"),
                InlineKeyboardButton.WithCallbackData("🔙 Asosiy menyu",   "menu:back")
            ]
        ]);

        var rate = await _currencyService.GetRateAsync(code);
        if (rate == null)
        {
            const string err = "❌ Bunday valyuta topilmadi. Qaytadan urinib ko'ring.";
            if (editMessageId.HasValue)
                await EditOrSend(botClient, chatId, editMessageId.Value, err, keyboard, ct);
            else
                await botClient.SendMessage(chatId, err, replyMarkup: keyboard, cancellationToken: ct);
            return;
        }

        DateTime today = DateTime.UtcNow.Date;
        var log = await _db.CurrencyLogs.FirstOrDefaultAsync(
            l => l.UserId == user.Id && l.CurrencyCode == code && l.Date == today, ct);

        if (log == null)
        {
            log = new CurrencyLog { UserId = user.Id, CurrencyCode = code, Date = today, ViewsCount = 1 };
            _db.CurrencyLogs.Add(log);
        }
        else log.ViewsCount++;

        await _db.SaveChangesAsync(ct);

        string result =
            $"💰 <b>1 {code} = {rate:N2} so'm</b>\n\n" +
            $"📅 Bugun <b>{log.ViewsCount}</b> marta ko'rdingiz.";

        if (editMessageId.HasValue)
            await EditOrSend(botClient, chatId, editMessageId.Value, result, keyboard, ct);
        else
            await botClient.SendMessage(chatId, result,
                parseMode: ParseMode.Html, replyMarkup: keyboard, cancellationToken: ct);
    }

    private async Task ShowAdminUsersAsync(ITelegramBotClient botClient,
        long chatId, int messageId, CancellationToken ct)
    {
        var users = await _db.Users.ToListAsync(ct);

        string res = "👥 <b>Foydalanuvchilar ro'yxati:</b>\n\n";
        var buttons = new List<IEnumerable<InlineKeyboardButton>>();

        foreach (var u in users)
        {
            res += $"• <code>{u.Id}</code> | {u.FullName} | {(u.IsBlocked ? "🔴 Bloklangan" : "🟢 Faol")}\n";

            // Har foydalanuvchi uchun blok/anblok tugmalari
            if (u.IsBlocked)
                buttons.Add([InlineKeyboardButton.WithCallbackData(
                    $"✅ {u.FullName} anblok", $"admin:unblock:{u.Id}")]);
            else
                buttons.Add([InlineKeyboardButton.WithCallbackData(
                    $"🚫 {u.FullName} blok", $"admin:block:{u.Id}")]);
        }

        buttons.Add([InlineKeyboardButton.WithCallbackData("🔙 Orqaga", "menu:back")]);
        await EditOrSend(botClient, chatId, messageId, res,
            new InlineKeyboardMarkup(buttons), ct);
    }

    private async Task ToggleBlockAsync(ITelegramBotClient botClient,
        long chatId, int messageId, long targetId, bool block, CancellationToken ct)
    {
        var target = await _db.Users.FindAsync(new object[] { targetId }, ct);
        if (target == null) return;

        target.IsBlocked = block;
        await _db.SaveChangesAsync(ct);

        string msg = block
            ? $"🚫 Foydalanuvchi <code>{targetId}</code> bloklandi."
            : $"✅ Foydalanuvchi <code>{targetId}</code> blokdan chiqarildi.";

        await ShowAdminUsersAsync(botClient, chatId, messageId, ct);
    }

    private async Task HandleAdminCommandsAsync(ITelegramBotClient botClient,
        long chatId, string text, CancellationToken ct)
    {
        var parts = text.Split(' ');
        var command = parts[0].ToLower();

        switch (command)
        {
            case "/users":
                var users = await _db.Users.ToListAsync(ct);
                string res = "👥 <b>Foydalanuvchilar:</b>\n\n";
                foreach (var u in users)
                    res += $"• <code>{u.Id}</code> | {u.FullName} | {(u.IsBlocked ? "🔴" : "🟢")}\n";
                await botClient.SendMessage(chatId, res,
                    parseMode: ParseMode.Html,
                    replyMarkup: AdminMenu,
                    cancellationToken: ct);
                break;

            case "/block" when parts.Length > 1 && long.TryParse(parts[1], out long bId):
                var bu = await _db.Users.FindAsync([bId], ct);
                if (bu != null) { bu.IsBlocked = true; await _db.SaveChangesAsync(ct); }
                await botClient.SendMessage(chatId, $"🚫 {bId} bloklandi.",
                    replyMarkup: AdminMenu, cancellationToken: ct);
                break;

            case "/unblock" when parts.Length > 1 && long.TryParse(parts[1], out long ubId):
                var ubu = await _db.Users.FindAsync([ubId], ct);
                if (ubu != null) { ubu.IsBlocked = false; await _db.SaveChangesAsync(ct); }
                await botClient.SendMessage(chatId, $"✅ {ubId} blokdan chiqarildi.",
                    replyMarkup: AdminMenu, cancellationToken: ct);
                break;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient,
        Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        => Task.CompletedTask;
}