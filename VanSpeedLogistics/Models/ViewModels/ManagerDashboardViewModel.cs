using VanSpeedLogistics.Models.Entities;

namespace VanSpeedLogistics.Models.ViewModels;


/// <summary>
/// ViewModel usada para transferir todos os dados necessários do ManagerController
/// para a View do Dashboard (Index.cshtml).
/// </summary>
public class ManagerDashboardViewModel
{
    public int PreviousDayDeliveries { get; set; }
    public int PreviousDayCollections { get; set; }
    public int PreviousDayReturns { get; set; }

    public IEnumerable<DeliveryRecord> RecentActivityLog { get; set; } = new List<DeliveryRecord>();
}