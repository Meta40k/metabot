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

        if ((context.ChatId == TelegramGroups.STORMSQUAD.ChatId && context.ThreadId == null) ||
            (context.ChatId == TelegramGroups.SANSARA.ChatId))
        {
            var totalIn = _db.Contributions.Sum(c => c.Amount);
            var totalOut = _db.Expenses.Sum(e => e.Amount);
            var balance = totalIn - totalOut;
            var den = _db.Persons.FirstOrDefault(person => person.Name == "Денис");


            if (den != null)
                await _bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text:
                    $"💳 *Счёт для пополнения:*\n\n" +
                    $"`+{den.PhoneNumber}`\n" +
                    $"{den.Bank} БАНК\n" +
                    $"получатель {den.Name} Л.\n" +
                    $"--------------------\n" +
                    $"*Баланс:* {balance}₽",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                );
        }
        
    }
}