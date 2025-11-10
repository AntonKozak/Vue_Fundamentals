using System.ComponentModel.DataAnnotations;

namespace Mango_Api.Models.Dto;

public class OrderHeaderCreateDto
{
    [Required]
    public string PickUpName { get; set; } = string.Empty;
    [Required]
    public string PickUpPhoneNumber { get; set; } = string.Empty;
    [Required]
    public string PickUpEmail { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;
    public double OrderTotal { get; set; }
    public int TotalItem { get; set; }

    public List<OrderDetailsCreateDto> OrderDetails { get; set; } = new();
}
