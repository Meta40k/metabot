using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using metabot.Models;

[Table("contributions", Schema = "bot_data")]
public class Contribution
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("storm_squad_id")]
    [ForeignKey("User")]
    public int StormSquadId { get; set; }

    public StormSquad User { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}