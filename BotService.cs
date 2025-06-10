using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace metabot;

public class BotService
{
    private readonly TelegramBotClient _botClient;
    private readonly AppDbContext _dbContext;

    public BotService(string token, AppDbContext dbContext)
    {
        _botClient = new TelegramBotClient(token);
        _dbContext = dbContext;
    }

    public async Task StartAsync()
    {
        var me = await _botClient.GetMeAsync();
        Console.WriteLine($"✅ Бот запущен: {me.FirstName} (@{me.Username})");

        using var cts = new CancellationTokenSource();

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandlePollingErrorAsync,
            new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() },
            cancellationToken: cts.Token
        );

        await Task.Delay(-1);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;

        Console.WriteLine($"📩 Новое сообщение от {message.From?.Username ?? "Неизвестный"}: {message.Text ?? "[Медиафайл]"}");

        // Проверяем, есть ли пользователь в БД
        var user = _dbContext.Users.FirstOrDefault(u => u.user_id == message.From.Id);
        if (user == null)
        {
            user = new User
            {
                user_id = message.From.Id,
                username = message.From.Username,
                first_name = message.From.FirstName,
                last_name = message.From.LastName
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        // Определяем тип сообщения
        string messageType = "text";
        string? fileId = null;
        long? fileSize = null;
        string? mimeType = null;

        if (message.Photo != null)
        {
            messageType = "photo";
            fileId = message.Photo[^1].FileId;
            fileSize = message.Photo[^1].FileSize;
            mimeType = "image/jpeg";
        }
        else if (message.Video != null)
        {
            messageType = "video";
            fileId = message.Video.FileId;
            fileSize = message.Video.FileSize;
            mimeType = message.Video.MimeType;
        }
        else if (message.Voice != null)
        {
            messageType = "voice";
            fileId = message.Voice.FileId;
            fileSize = message.Voice.FileSize;
            mimeType = message.Voice.MimeType;
        }
        else if (message.Document != null)
        {
            messageType = "document";
            fileId = message.Document.FileId;
            fileSize = message.Document.FileSize;
            mimeType = message.Document.MimeType;
        }

        // Сохраняем сообщение
        var newMessage = new Message
        {
            chat_id = message.Chat.Id,
            user_id = user.user_id,
            message_type = messageType,
            message_text = message.Text,
            file_id = fileId,
            file_size = fileSize,
            mime_type = mimeType
        };
        _dbContext.Messages.Add(newMessage);
        await _dbContext.SaveChangesAsync();
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"❌ Ошибка Telegram API: {exception.Message}");
        return Task.CompletedTask;
    }
}