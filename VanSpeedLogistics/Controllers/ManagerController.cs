using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanSpeedLogistics.Data;

namespace VanSpeedLogistics.Controllers;

/// <summary>
/// Controller dedicado à área administrativa do Gestor de Frota (Role: Manager).
/// O atributo [Authorize] garante que apenas usuários autenticados e com a Role "Manager"
/// possam acessar qualquer Action dentro deste Controller.
/// </summary>
[Authorize(Roles = "Manager")]
public class ManagerController : Controller
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Construtor que recebe o ApplicationDbContext por Injeção de Dependência.
    /// Isso permite que o Controller acesse todas as tabelas e dados do sistema.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    public ManagerController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Action Index (GET) que serve como o Dashboard principal do Gestor.
    /// Por enquanto, apenas exibe a View, mas será o ponto de partida para os KPIs.
    /// </summary>
    /// <returns>A View Index, que será o Dashboard.</returns>
    public IActionResult Index()
    {
        return View();
    }
}