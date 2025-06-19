using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using metabot.Models;
using metabot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = metabot.Models.Message;

namespace metabot;

public class BotService
{
    private readonly TelegramBotClient _botClient;
    private readonly AppDbContext _dbContext;
    private readonly IChatContextProvider _chatContextProvider;


    public BotService(string token, AppDbContext dbContext, IChatContextProvider contextProvider)
    {
        _botClient = new TelegramBotClient(token);
        _dbContext = dbContext;
        _chatContextProvider = contextProvider;

        TelegramGroups.STORMSQUAD = _dbContext.TelegramGroups.First(group => group.Title == "STORMSQUAD");
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

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { } message) return;

        Console.WriteLine(
            $"📩 Новое сообщение от {message.From?.Username ?? "Неизвестный"}: {message.Text ?? "[Медиафайл]"}");

        var dispatcher = new CommandDispatcher(_dbContext, _botClient, _chatContextProvider);
        await dispatcher.HandleAsync(message);

        Console.WriteLine(message.Text);

        if (message.Chat.Id == TelegramGroups.STORMSQUAD.ChatId)
        {
            if (message.From == null) return;
            
            var stormUser = _dbContext.StormSquad
                .FirstOrDefault(stormUser => message.From != null && stormUser.UserId == message.From.Id);
            
            if (stormUser == null)
            {
                stormUser = new StormSquad
                {
                    UserId = message.From.Id,
                    Username = message.From.Username,
                    FirstName = message.From.FirstName,
                    LastName = message.From.LastName
                };
                _dbContext.StormSquad.Add(stormUser);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

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
            await _dbContext.SaveChangesAsync(cancellationToken);
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
            ChatId = message.Chat.Id,
            UserId = user.user_id,
            MessageType = messageType,
            MessageText = message.Text,
            FileId = fileId,
            FileSize = fileSize,
            MimeType = mimeType,
            Username = message.From?.Username,
            ThreadId = message.MessageThreadId
        };
        _dbContext.Messages.Add(newMessage);
        await _dbContext.SaveChangesAsync();
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("❌ Ошибка Telegram API: " + exception.Message);

        if (exception.InnerException != null)
            Console.WriteLine("🔍 Внутренняя ошибка: " + exception.InnerException.Message);

        return Task.CompletedTask;
    }
}