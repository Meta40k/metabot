using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using metabot.Models;
using metabot.Services;

namespace metabot.Commands;

public class BalanceCommand
{
    private readonly AppDbContext _db;
    private readonly ITelegramBotClient _bot;
    private readonly IChatContextProvider _chatContextProvider;

    public BalanceCommand(AppDbContext db, ITelegramBotClient bot, IChatContextProvider contextProvider)
    {
        _db = db;
        _bot = bot;
        _chatContextProvider = contextProvider;
    }

    public async Task ExecuteAsync(Telegram.Bot.Types.Message message)
    {
        var context = _chatContextProvider.GetContext(message);

        // Допустим, только в суперчате StormSquad разрешён просмотр баланса
        //if (context.ChatId == -1002878092364 && context.ThreadId == null)
        if (context.ChatId == -1002878092364 && context.ThreadId == null)
        {
            var totalIn = _db.Contributions.Sum(c => c.Amount);
            var totalOut = _db.Expenses.Sum(e => e.Amount);
            var balance = totalIn - totalOut;
            
            await _bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text:
                $"💰 *Баланс StormSquad:*\n\n" +
                $"Внесено: {totalIn}₽\n" +
                $"Потрачено: {totalOut}₽\n" +
                $"--------------------\n" +
                $"*Итого:* {balance}₽",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
        }

        Console.WriteLine(context.ChatId);
        Console.WriteLine(context.ThreadId);
        Console.WriteLine(context.Username);
        


    }
}