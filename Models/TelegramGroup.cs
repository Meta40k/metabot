using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace metabot.Models;

[Table("telegram_groups", Schema = "bot_data")]
public class TelegramGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("chat_id")]
    public long ChatId { get; set; }
    
    [Column("title")]
    public string? Title { get; set; }
    
    [Column("username")]
    public string? Username { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}