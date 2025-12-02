using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VanSpeedLogistics.Data;
using Microsoft.EntityFrameworkCore;
using VanSpeedLogistics.Models.ViewModels;
using ClosedXML.Excel;
using System.IO;




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

    /// <summary>
    /// Action que exporta o Log de Atividades completo da Frota para um arquivo Excel (.xlsx) usando ClosedXML.
    /// </summary>
    /// <returns>Um arquivo FileContentResult para download do navegador.</returns>
    [HttpGet]
    public async Task<IActionResult> ExportToExcel()
    {
        var records = await _context.DeliveryRecords
            .Include(r => r.User)
            .OrderByDescending(r => r.Date)
            .ToListAsync();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Flee Activity Log");
            // Definição do Cabeçalho
            worksheet.Cell("A1").Value = "Driver Name";
            worksheet.Cell("B1").Value = "Registration Date";
            worksheet.Cell("C1").Value = "Deliveries";
            worksheet.Cell("D1").Value = "Collections";
            worksheet.Cell("E1").Value = "Returns";
            worksheet.Cell("F1").Value = "Notes";

            // Formatação do Cabeçalho
            var headerRange = worksheet.Range("A1:F1");
            headerRange.Style.Font.SetBold();
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            int currentRow = 2;
            foreach (var record in records)
            {
                worksheet.Cell(currentRow, 1).Value = record.User?.FullName ?? "N/A";

                worksheet.Cell(currentRow, 2).Value = record.Date.ToString("yyyy-MM-dd HH:mm");
                worksheet.Cell(currentRow, 3).Value = record.Deliveries;
                worksheet.Cell(currentRow, 4).Value = record.Collections;
                worksheet.Cell(currentRow, 5).Value = record.Returns;
                worksheet.Cell(currentRow, 6).Value = record.Notes;

                currentRow++;
            }

            // Formatação da planilha, ajustando o tamanho das colunas e centralização no caso dos dados nas colunas C:E
            worksheet.Columns().AdjustToContents();
            worksheet.Columns("C:E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                return File(
                    content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"VanSpeed_Fleet_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                );
            }
        }
    }
}