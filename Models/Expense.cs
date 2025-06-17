using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("expenses", Schema = "bot_data")]
public class Expense
{
    [Key]
    public int Id { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("receipt_date")]
    public DateTime ReceiptDate { get; set; }
}