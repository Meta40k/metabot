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
