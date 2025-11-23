using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress]
    public string Email{get;set;} =  string.Empty;
    
    [Required(ErrorMessage = "A senha é obrigatória")]
    [DataType(DataType.Password)]
    public string Password{get;set;} =   string.Empty;
    
    [Display(Name = "Lembrar de mim?")]
    public bool RememberMe { get; set; }
}