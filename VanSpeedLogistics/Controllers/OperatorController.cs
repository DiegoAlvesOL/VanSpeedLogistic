using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VanSpeedLogistics.Controllers;

/// <summary>
/// Este Controller é dedicado às funcionalidades do Operador Logístico.
/// A annotation [Authorize(Roles = "Operator,Manager")] garante que apenas usuários
/// com os papeis "Operator" ou "Manager" podem acessar qualquer método (Action) dentro desta classe.
/// </summary>
[Authorize(Roles = "Operator, Manager")]
public class OperatorController : Controller
{
    /// <summary>
    /// Action Index (GET): Responsável por carregar o formulário de lançamento de dados.
    /// Retorna a View onde o operador irá preencher seus registros diários.
    /// </summary>
    /// <returns>A View contendo o formulário diário de registro.</returns>
    public IActionResult Index()
    {
        return View();
    }
    
}