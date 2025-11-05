using Microsoft.AspNetCore.Identity;

namespace Mango_Api.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
