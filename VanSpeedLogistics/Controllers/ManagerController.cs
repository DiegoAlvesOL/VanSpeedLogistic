using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanSpeedLogistics.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using VanSpeedLogistics.Models.ViewModels;
using System.Linq;
using VanSpeedLogistics.Models.Entities;


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
    public async Task<IActionResult> Index()
    {
        // Definição do Intervalo de Tempo (Dia Anterior)
        // Os operadores postam no final do dia, então os KPIs se referem ao dia anterior (Yesterday).
        
        // Pega a data de hoje (meia-noite)
        var today = DateTime.Today;
        
        // Data de início do dia anterior (Yesterday Start: ex: 2025-11-30 00:00:00)
        var yesterdayStart = today.AddDays(-1);
        
        // Data de fim do dia anterior (Yesterday End: ex: 2025-11-30 23:59:59.999)
        var yesterdayEnd = today.AddTicks(-1);
        
        // Cria a ViewModel
        var viewModel = new ManagerDashboardViewModel();
        
        // Consulta ao Banco de Dados (Cálculo dos KPIs do Dia Anterior)
        // Filtra todos os registros que ocorreram no intervalo do dia anterior

        var previousDayRecords = _context.DeliveryRecords
            .Where(r => r.Date >= yesterdayStart && r.Date <= yesterdayEnd);

        viewModel.PreviousDayDeliveries = await previousDayRecords
            .SumAsync(r => r.Deliveries);

        viewModel.PreviousDayCollections = await previousDayRecords
            .SumAsync(r => r.Collections);

        viewModel.PreviousDayReturns = await previousDayRecords
            .SumAsync(r => r.Returns);
        
        // Consulta do Log de Atividades Recentes
        // Busca os 20 últimos registros da frota, ordenados de forma decrescente
        // Inclui o ApplicationUser (propriedade User) para ter acesso ao FullName do operador no Log
        viewModel.RecentActivityLog = await _context.DeliveryRecords
            .Include(r => r.User)
            .OrderByDescending(r => r.Date)
            .Take(20)
            .ToListAsync();
        
        // Passa a ViewModel preenchida para a View
        return View(viewModel);
    }
}