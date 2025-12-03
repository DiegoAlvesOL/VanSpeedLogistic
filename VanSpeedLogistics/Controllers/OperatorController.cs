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

        // Montamos uma consulta LINQ para buscar o histórico do usuário (últimos 10 registros)
        var userHistory = await _context.DeliveryRecords
            .Where(r => r.DriverId == user.Id)
            .OrderByDescending(r => r.Date)
            .Take(10)
            .ToListAsync();
        
        ViewBag.History = userHistory;
        
        // Retorna a View (sem model na requisição GET, o model é criado vazio)
        return View();

    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DeliveryViewModel model)
    {
        // 1. CORREÇÃO DE VALIDAÇÃO: Se o modelo for inválido, precisamos recarregar o ViewBag.History
        // Se a validação falhar, a View precisa do ViewBag.History para evitar que a tabela de histórico desapareça.
        if (!ModelState.IsValid)
        {
            // OBTEM O USUÁRIO E O HISTÓRICO NOVAMENTE
            var user = await _userManager.GetUserAsync(User);
            
            var userHistory = await _context.DeliveryRecords
                .Where(r => r.DriverId == user.Id)
                .OrderByDescending(r => r.Date)
                .Take(10)
                .ToListAsync();
            
            ViewBag.History = userHistory;
            
            // Retorna a View COM o model (para que as mensagens de erro sejam exibidas)
            return View("Index", model);
        }
        
        // -- Lógica de Sucesso --
        
        // Identificando quem é o motorista logado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Transformar ViewModel em um objeto final que vai para o MySQL.
        var record = new DeliveryRecord
        {
            DriverId = userId,
            Date = DateTime.Now,
            Deliveries = model.Deliveries,
            Collections = model.Collections,
            Returns = model.Returns,
            Notes = model.Notes
        };
        
        _context.DeliveryRecords.Add(record);
        
        // Commita a transação no MySQL.
        await _context.SaveChangesAsync();
        
        // Define a mensagem de sucesso que será lida pelo TempData na View
        TempData["SuccessMessage"] = "Registro diário salvo com sucesso!";
        
        // 2. CORREÇÃO DE REDIRECIONAMENTO: Redireciona para o Index deste Controller
        // O RedirectToAction dispara o método [HttpGet] Index() (que recarrega a página)
        // e garante que o TempData["SuccessMessage"] seja exibido na página correta.
        return RedirectToAction(nameof(Index)); // Equivalente a RedirectToAction("Index")
    }
    
}