using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VanSpeedLogistics.Models;

namespace VanSpeedLogistics.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    

    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "invalid login attempt. Please try again or check your email.");
        }
        return View(model);
    }

    /// <summary>
    /// Action Register (GET): Exibe o formulário de cadastro de novo motorista.
    /// A restrição [Authorize(Roles = "Manager")] garante que apenas usuários com a role Manager acessem esta rota.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    /// <summary>
    /// Action Register (POST): Recebe os dados do formulário, cria o ApplicationUser, salva no banco e atribui a Role "Operator".
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // Validação: Verifica se todas as regras da RegisterViewModel foram atendidas.
        if (ModelState.IsValid)
        {
            //Mapeamento (ViewModel -> Entidade de Banco):
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true
            };
            
            
            // Criação do Usuário no Identity: Salva no MySQL e criptofrafa a senha.
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Atribuição de Role: Define o novo usuário como Operator.
                await _userManager.AddToRoleAsync(user, "Operator");
                
                
                // Sucesso: Armazena uma mensagem temporária para a próxima requisição (redirect)
                TempData["SuccessMessage"] = $"Driver {user.FullName} successfully registered as operator.";
                
                // Redireciona para a Action GET (limpa o formulário)
                return RedirectToAction("Register");
            }

            // Se a criação falhar (ex: email já existe), adiciona os erros ao ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        // Se a validação inicial (ou a criação) falhar, retorna a View com os dados preenchidos e os erros.
        return View(model);
    }

    /// <summary>
    /// Método temporário para forçar a redefinição de senha de um motorista existente
    /// para uma senha conhecida, corrigindo problemas de hash/login.
    /// Este método deve ser removido após o desenvolvimento.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> ResetDriverPassword(string email, string newPassword)
    {
        // Garante que Peter está logado.
        if (User.Identity == null || !User.IsInRole("Manager"))
        {
            return Forbid();
        }

        // Localiza o usuário alvo.
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["ErrorMessage"] = $"User with email {email} not found.";
            return RedirectToAction("Register");
        }
        
        
        // Remove e gera um novo token de reset. (Necessário para a função ResetPasswordAsync)
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        // Força a nova senha. O Identity cuida da criptografia (hash) e salva no banco.
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);


        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = $"Password for {user.FullName} successfully reset to {newPassword}.";
            return RedirectToAction("Register");
        }
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        TempData["ErrorMessage"] = $"Failed to reset password: {errors}";
        return RedirectToAction("Register");
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}