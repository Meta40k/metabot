using metabot.Models;
using Telegram.Bot.Types;

namespace metabot.Services;

public interface IChatContextProvider
{
    ChatContext GetContext(Telegram.Bot.Types.Message message);
}