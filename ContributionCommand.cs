using metabot.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

// если модели лежат отдельно

namespace metabot;

public class ContributionCommand
{
    private readonly AppDbContext _db;
    private readonly ITelegramBotClient _bot;

    public ContributionCommand(AppDbContext db, ITelegramBotClient bot)
    {
        _db = db;
        _bot = bot;
    }

    public async Task ExecuteAsync(Telegram.Bot.Types.Message message)
    {
        var parts = message.Text!.Split(' ', 4, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
        {
            await _bot.SendTextMessageAsync(message.Chat.Id, "❌ Формат: /взнос @username 500 комментарий");
            return;
        }

        var username = parts[1].TrimStart('@');
        if (!decimal.TryParse(parts[2], out var amount))
        {
            await _bot.SendTextMessageAsync(message.Chat.Id, "❌ Некорректная сумма");
            return;
        }

        var comment = parts.Length >= 4 ? parts[3] : "";

        var user1 = await _db.Users.FirstOrDefaultAsync(u => u.username.ToLower() == username.ToLower());
        var user = await _db.StormSquad.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        
        if (user == null)
        {
            user = new StormSquad()
            {
                Username = username,
                FirstName = username
            };
            _db.StormSquad.Add(user);
            await _db.SaveChangesAsync();
        }

        var contribution = new Contribution
        {
            StormSquadId = user.Id,
            Amount = amount,
            Comment = comment
        };
        _db.Contributions.Add(contribution);
        await _db.SaveChangesAsync();

        await _bot.SendTextMessageAsync(message.Chat.Id,
            $"✅ Взнос принят: @{user.Username}, {amount}₽. {comment}");
    }
}