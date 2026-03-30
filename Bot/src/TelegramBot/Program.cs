using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public class Program
{
    static string Month1 = "fevral";
    static string Month2 = "mart";

    // Toshkent vaqtlari: kun → (saharlik, iftorlik)
    static readonly Dictionary<string, (string Sahar, string Iftor)> RamadanTimes = new()
    {
        { "19", ("05:12", "18:21") },
        { "20", ("05:11", "18:22") },
        { "21", ("05:09", "18:23") },
        { "22", ("05:08", "18:24") },
        { "23", ("05:06", "18:25") },
        { "24", ("05:05", "18:26") },
        { "25", ("05:03", "18:27") },
        { "26", ("05:02", "18:28") },
        { "27", ("05:00", "18:29") },
        { "28", ("04:59", "18:30") },
        { "1",  ("04:57", "18:31") },
        { "2",  ("04:55", "18:32") },
        { "3",  ("04:54", "18:33") },
        { "4",  ("04:52", "18:34") },
        { "5",  ("04:51", "18:35") },
        { "6",  ("04:49", "18:36") },
        { "7",  ("04:47", "18:37") },
        { "8",  ("04:46", "18:38") },
        { "9",  ("04:44", "18:39") },
        { "10", ("04:42", "18:40") },
        { "11", ("04:41", "18:41") },
        { "12", ("04:39", "18:42") },
        { "13", ("04:37", "18:43") },
        { "14", ("04:36", "18:44") },
        { "15", ("04:34", "18:45") },
        { "16", ("04:32", "18:46") },
        { "17", ("04:31", "18:47") },
        { "18", ("04:29", "18:48") },
    };

    private static InlineKeyboardMarkup MainMenu => new(
    [
        [
            InlineKeyboardButton.WithCallbackData($"19 - {Month1}", "19"),
            InlineKeyboardButton.WithCallbackData($"20 - {Month1}", "20"),
            InlineKeyboardButton.WithCallbackData($"21 - {Month1}", "21")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"22 - {Month1}", "22"),
            InlineKeyboardButton.WithCallbackData($"23 - {Month1}", "23"),
            InlineKeyboardButton.WithCallbackData($"24 - {Month1}", "24")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"25 - {Month1}", "25"),
            InlineKeyboardButton.WithCallbackData($"26 - {Month1}", "26"),
            InlineKeyboardButton.WithCallbackData($"27 - {Month1}", "27")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"28 - {Month1}", "28"),
            InlineKeyboardButton.WithCallbackData($"1 - {Month2}",  "1"),
            InlineKeyboardButton.WithCallbackData($"2 - {Month2}",  "2")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"3 - {Month2}",  "3"),
            InlineKeyboardButton.WithCallbackData($"4 - {Month2}",  "4"),
            InlineKeyboardButton.WithCallbackData($"5 - {Month2}",  "5")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"6 - {Month2}",  "6"),
            InlineKeyboardButton.WithCallbackData($"7 - {Month2}",  "7"),
            InlineKeyboardButton.WithCallbackData($"8 - {Month2}",  "8")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"9 - {Month2}",  "9"),
            InlineKeyboardButton.WithCallbackData($"10 - {Month2}", "10"),
            InlineKeyboardButton.WithCallbackData($"11 - {Month2}", "11")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"12 - {Month2}", "12"),
            InlineKeyboardButton.WithCallbackData($"13 - {Month2}", "13"),
            InlineKeyboardButton.WithCallbackData($"14 - {Month2}", "14")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"15 - {Month2}", "15"),
            InlineKeyboardButton.WithCallbackData($"16 - {Month2}", "16"),
            InlineKeyboardButton.WithCallbackData($"17 - {Month2}", "17")
        ],
        [
            InlineKeyboardButton.WithCallbackData($"18 - {Month2}", "18")
        ]
    ]);

    // Orqaga tugmasi
    private static InlineKeyboardMarkup BackMenu => new(
    [
        [InlineKeyboardButton.WithCallbackData("🔙 Kunlar ro'yxatiga qaytish", "back")]
    ]);

    static async Task Main(string[] args)
    {
        string botToken = Environment.GetEnvironmentVariable("MY_BOT_TOKEN")!;

        TelegramBotClient telegramBot = new TelegramBotClient(botToken);

        var handler = new Program();

        telegramBot.StartReceiving(
            updateHandler: handler.HandleUpdateAsync,
            errorHandler: handler.HandleErrorAsync,
            cancellationToken: CancellationToken.None
        );

        Console.WriteLine("Bot ishga tushdi ✅");
        Console.ReadKey();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken)
    {
        // Inline tugma bosildi
        if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            await HandleCallbackAsync(botClient, update.CallbackQuery, cancellationToken);
            return;
        }

        // Oddiy xabar keldi
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text.Trim();

            if (text == "/start")
            {
                await botClient.SendMessage(
                    chatId,
                    "🌙 <b>Ramazon 2026 — Toshkent vaqtlari</b>\n\nQuyidan kunni tanlang:",
                    parseMode: ParseMode.Html,
                    replyMarkup: MainMenu,
                    cancellationToken: cancellationToken
                );
            }
        }
    }

    private async Task HandleCallbackAsync(ITelegramBotClient botClient,
        CallbackQuery callback, CancellationToken ct)
    {
        var chatId = callback.Message!.Chat.Id;
        var messageId = callback.Message.MessageId;
        var data = callback.Data ?? string.Empty;

        // "yuklanyapti..." ko'rsatmaslik uchun
        await botClient.AnswerCallbackQuery(callback.Id, cancellationToken: ct);

        // Orqaga tugmasi
        if (data == "back")
        {
            await botClient.EditMessageText(
                chatId,
                messageId,
                "🌙 <b>Ramazon 2026 — Toshkent vaqtlari</b>\n\nQuyidan kunni tanlang:",
                parseMode: ParseMode.Html,
                replyMarkup: MainMenu,
                cancellationToken: ct
            );
            return;
        }

        // Kun tanlandi
        if (RamadanTimes.TryGetValue(data, out var times))
        {
            // Oyni aniqlash
            string oy = int.Parse(data) >= 19 && int.Parse(data) <= 28
                ? Month1
                : Month2;

            string text =
                $"📅 <b>{data} - {oy}</b>\n\n" +
                $"🌅 Saharlik:  <b>{times.Sahar}</b>\n" +
                $"🌇 Iftorlik:  <b>{times.Iftor}</b>\n\n" +
                $"🕌 Vaqtlar Toshkent bo'yicha";

            await botClient.EditMessageText(
                chatId,
                messageId,
                text,
                parseMode: ParseMode.Html,
                replyMarkup: BackMenu,
                cancellationToken: ct
            );
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient,
        Exception exception, HandleErrorSource source, CancellationToken ct)
    {
        Console.WriteLine($"Xato: {exception.Message}");
        return Task.CompletedTask;
    }
}