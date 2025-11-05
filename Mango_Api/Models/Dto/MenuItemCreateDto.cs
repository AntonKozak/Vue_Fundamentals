namespace Mango_Api.Models.Dto;

public class MenuItemCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? SpecialTag { get; set; }
    public double Price { get; set; }
    public IFormFile File { get; set; } = null!;
}
