using metabot.Models;
using Telegram.Bot.Types;

namespace metabot.Services;

public class ChatContextProvider : IChatContextProvider
{
    public ChatContext GetContext(Telegram.Bot.Types.Message message)
    {
        var chat = message.Chat;

        return new ChatContext
        {
            ChatId = chat.Id,
            ChatTitle = chat.Title,
            Username = chat.Username,
            ThreadId = message.MessageThreadId,
            Type = chat.Type switch
            {
                Telegram.Bot.Types.Enums.ChatType.Private => ChatType.Private,
                Telegram.Bot.Types.Enums.ChatType.Group => ChatType.Group,
                Telegram.Bot.Types.Enums.ChatType.Supergroup => ChatType.Supergroup,
                Telegram.Bot.Types.Enums.ChatType.Channel => ChatType.Channel,
                _ => ChatType.Unknown
            }
        };
    }
}