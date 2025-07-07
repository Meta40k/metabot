using System.Text;
using metabot.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace metabot.Commands;

public class WhoDidNotContributeCommand
{
    private readonly AppDbContext _db;
    private readonly ITelegramBotClient _bot;
    private readonly IChatContextProvider _chatContextProvider;

    public WhoDidNotContributeCommand(AppDbContext db, ITelegramBotClient botClient, IChatContextProvider contextProvider)
    {
        _db = db;
        _bot = botClient;
        _chatContextProvider = contextProvider;
        
    }

    public async Task ExecuteAsync(Telegram.Bot.Types.Message message)
    {
        var context = _chatContextProvider.GetContext(message);
        
        if ((context.ChatId == TelegramGroups.STORMSQUAD.ChatId && context.ThreadId == null) ||
            (context.ChatId == TelegramGroups.SANSARA.ChatId))
        {
            var allTags = await   _db.Contributions
                .Where(c => EF.Functions.Like(c.Comment, "Общий сбор%"))
                .Select(c => c.Comment)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        
        
            var allUsers = await _db.StormSquad.ToListAsync();
        
        
            var sb = new StringBuilder();
        
            foreach (var tag in allTags)
            {
                var paidUserIds = await _db.Contributions
                    .Where(c => c.Comment == tag)
                    .Select(c => c.StormSquadId)
                    .Distinct()
                    .ToListAsync();

                var nonPayers = allUsers
                    .Where(u => u.IsContributing) // ← только те, кто должен участвовать
                    .Where(u => !paidUserIds.Contains(u.Id))
                    .ToList();


                if (nonPayers.Count == 0)
                {
                    sb.AppendLine($"✅ *{tag}*: Все сдали");
                }
                else
                {
                    sb.AppendLine($"🚫 *{tag}*");
                    foreach (var u in nonPayers)
                    {
                        sb.AppendLine($"{u.FirstName}" +
                                      (string.IsNullOrWhiteSpace(u.Username) ? "" : $" (@{u.Username})"));
                    }
                    sb.AppendLine();
                }
            }

            sb.Append("----------\n");
            sb.Append("Узнать реквизиты\n");
            sb.Append("Команда:   /счет\n");
        
        
            await _bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: sb.ToString()
            );
        }
        
    }
}