using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace metabot.Models;

[Table("stormsquad", Schema = "bot_data")]
public class StormSquad
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("user_id")]
    public long UserId { get; set; }
    
    [Column("username")]
    public string? Username { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string? LastName { get; set; }
    
    [Column("iscontributing")]
    public bool IsContributing { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}