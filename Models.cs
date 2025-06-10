using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace metabot;

[Table("users", Schema = "bot_data")]
public class User
{
    [Key]
    public long user_id { get; set; }
    public string? username { get; set; }
    public string? first_name { get; set; }
    public string? last_name { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}

[Table("messages", Schema = "bot_data")]
public class Message
{
    [Key]
    public int id { get; set; }
    public long chat_id { get; set; }
    
    [ForeignKey("User")]
    public long user_id { get; set; }
    public User User { get; set; }

    public string message_type { get; set; } = "text";
    public string? message_text { get; set; }
    public string? file_id { get; set; }
    public long? file_size { get; set; }
    public string? mime_type { get; set; }
    public DateTime sent_at { get; set; } = DateTime.UtcNow;
}