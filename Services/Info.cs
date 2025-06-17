using Telegram.Bot;

namespace metabot.Services;

public class Info
{
    private readonly AppDbContext _db;
    private readonly ITelegramBotClient _bot;
    private readonly IChatContextProvider _chatContextProvider;

    public Info(AppDbContext db, ITelegramBotClient bot, IChatContextProvider chatContextProvider)
    {
        _db = db;
        _bot = bot;
        _chatContextProvider = chatContextProvider;
    }

    public async Task GetInvoice(Telegram.Bot.Types.Message message)
    {
        var context = _chatContextProvider.GetContext(message);

        if (context.ChatId == -1002878092364 && context.ThreadId == null)
        {
            var totalIn = _db.Contributions.Sum(c => c.Amount);
            var totalOut = _db.Expenses.Sum(e => e.Amount);
            var balance = totalIn - totalOut;
            
            await _bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text:
                $"💳 *Счёт для пополнения:*\n\n" +
                $"`+79300630127`\n" +
                $"OZON БАНК\n" +
                $"получатель Денис Л.\n" +
                $"--------------------\n" +
                $"*Баланс:* {balance}₽",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
        }
        
    }
}