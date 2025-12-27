using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

public class AdminResetViewModel
{
    [Required(ErrorMessage = "The email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "The password must be at least 6 characters long.")]
    public string NewPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}