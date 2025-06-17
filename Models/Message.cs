using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace metabot.Models
{
    [Table("messages", Schema = "bot_data")]
    public class Message
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("chat_id")]
        public long ChatId { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public long UserId { get; set; }

        public User User { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("thread_id")]
        public int? ThreadId { get; set; }

        [Column("message_type")]
        public string MessageType { get; set; } = "text";

        [Column("message_text")]
        public string? MessageText { get; set; }

        [Column("file_id")]
        public string? FileId { get; set; }

        [Column("file_size")]
        public long? FileSize { get; set; }

        [Column("mime_type")]
        public string? MimeType { get; set; }

        [Column("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}