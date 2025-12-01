using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Full name is required.")]
    [Display(Name = "Full name")]
    [StringLength(150, ErrorMessage = "Maximum length is {1}")]
    public string FullName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } =  string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Temporary Password")]
    public string Password { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}