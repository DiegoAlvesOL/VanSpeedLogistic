using Microsoft.AspNetCore.Identity;
namespace VanSpeedLogistics.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName {get; set;} = string.Empty;
}