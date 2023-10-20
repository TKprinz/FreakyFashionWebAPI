using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FreakyFashionWebAPI.Domain;




[Index(nameof(StockKeepingUnit), IsUnique = true)]

public class Product

{

    public int Id { get; set; }
    [MaxLength(50)]
    public required string ProductName { get; set; }
    [Column(TypeName = "nchar(50)")]
    public required string StockKeepingUnit { get; set; }
    [MaxLength(150)]
    public required string Description { get; set; }
    [MaxLength(200)]
    public required string Image { get; set; }
    [MaxLength(15)]
    public required string Price { get; set; }

}