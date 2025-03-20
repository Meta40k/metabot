using Telegram.Bot;

namespace metabot;

public class BotService
{
    private TelegramBotClient _botClient;

    public BotService(string token)
    {
        _botClient = new TelegramBotClient(token);
    }

    public async Task StartAsync()
    {
        
    }
}