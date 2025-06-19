using metabot.Commands;
using metabot.Services;
using Telegram.Bot;

namespace metabot;

public class CommandDispatcher
{
    private readonly AppDbContext _db;
    private readonly ITelegramBotClient _bot;
    private readonly IChatContextProvider _chatContextProvider;


    public CommandDispatcher(AppDbContext dbContext, ITelegramBotClient botClient, IChatContextProvider chatContextProvider)
    {
        _db = dbContext;
        _bot = botClient;
        _chatContextProvider = chatContextProvider;
    }

    public async Task HandleAsync(Telegram.Bot.Types.Message message)

    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (message.Text == null) return;

        if (message.Text.StartsWith("/взнос"))
        {
            Console.WriteLine("ВЗНОС");
            var handler = new ContributionCommand(_db, _bot);
            await handler.ExecuteAsync(message);
        }
        else if (message.Text.StartsWith("/баланс"))
        {
            var handler = new BalanceCommand(_db, _bot, _chatContextProvider);
            await handler.ExecuteAsync(message);
        }
        else if (message.Text.StartsWith("/счет") || message.Text.StartsWith("/счёт"))
        {
           var handler = new Info(_db, _bot, _chatContextProvider);
           await handler.GetInvoice(message);
        }
        else if (message.Text.StartsWith("/долг"))
        {
            var handler = new WhoDidNotContributeCommand(_db, _bot);
            await handler.ExecuteAsync(message);
        }

        else
        {
            // если будет нужно — логика по умолчанию
        }
    }
}