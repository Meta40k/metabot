namespace metabot.Models;

public class ChatContext
{
    public long ChatId { get; set; }
    public string? ChatTitle { get; set; } // для групп
    public string? Username { get; set; }  // для лички
    public int? ThreadId { get; set; } //ID темы
    public ChatType Type { get; set; }
}

public enum ChatType
{
    Private,
    Group,
    Supergroup,
    Channel,
    Unknown
}