using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace metabot.Models;
[Table("persons", Schema = "bot_data")]
public class Person
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Column("phone")]
    public long PhoneNumber { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("bank")]
    public string? Bank { get; set; }
    
    [Column("text1")]
    public string? Text1 { get; set; }
        
    [Column("text2")]
    public string? Text2 { get; set; }
        
    [Column("text3")]
    public string? Text3 { get; set; }
    
}