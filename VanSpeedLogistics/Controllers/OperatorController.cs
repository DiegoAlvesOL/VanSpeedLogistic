using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VanSpeedLogistics.Data;
using VanSpeedLogistics.Models;
using VanSpeedLogistics.Models.Entities;


namespace VanSpeedLogistics.Controllers;

/// <summary>
/// Este Controller é dedicado às funcionalidades do Operador Logístico.
/// A annotation [Authorize(Roles = "Operator,Manager")] garante que apenas usuários
/// com os papeis "Operator" ou "Manager" podem acessar qualquer método (Action) dentro desta classe.
/// </summary>
[Authorize(Roles = "Operator, Manager")]
public class OperatorController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OperatorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    /// <summary>
    /// Action Index (GET): Responsável por carregar o formulário de lançamento de dados.
    /// Retorna a View onde o operador irá preencher seus registros diários.
    /// </summary>
    /// <returns>A View contendo o formulário diário de registro.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("Usuário não encontrado");
        }

        // Montamos uma consulta SQL usando C# (LINQ)
        var userHistory = await _context.DeliveryRecords
            .Where(r => r.DriverId == user.Id)
            .OrderByDescending(r => r.Date)
            .Take(10)
            .ToListAsync();
        
        ViewBag.History = userHistory;
        
        return View();

    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DeliveryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        
        // Identificando quem é o motorista logado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Transformar ViewModel em um objeto final que vai para o MySQL.
        var record = new DeliveryRecord
        {
            DriverId = userId,
            Date = DateTime.Now,
            DeliveriesCount = model.DeliveriesCount,
            CollectionsCount = model.CollectionsCount,
            RetournsCount = model.RetournsCount,
            Notes = model.Notes
        };
        // o processo de gravar os dados no banco começar aqui onde estou escrevendo os dados na memória
        _context.DeliveryRecords.Add(record);
        
        // Esse trecho commita a transação no MySQL, gerando o insert into.
        await _context.SaveChangesAsync();
        
        // Redirecionando o usuário par auma página de sucesso.
        TempData["SuccessMessage"] = "Record added successfully";
        return RedirectToAction("Index", "Home");
    }
    
}