using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango_Api.Models;

public class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }
    public int OrderHeaderId { get; set; }
    public int MenuItemId { get; set; }
    [ForeignKey("MenuItemId")]
    public MenuItem? MenuItem { get; set; }

    [Required]
    public int Quantity { get; set; }
    [Required]
    public string ItemName { get; set; } = string.Empty;
    [Required]
    public double Price { get; set; }
    public int? Rating { get; set; }
}
