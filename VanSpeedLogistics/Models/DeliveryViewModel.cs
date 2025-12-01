using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

public class DeliveryViewModel
{
    [Display(Name = "Completed Deliveries")]
    [Required(ErrorMessage = "Please inform the number of deliveries.")]
    [Range(0,999,ErrorMessage = "The value must be positive.")]
    public int DeliveriesCount { get; set; }
    
    [Display(Name = "Collections")]
    [Required(ErrorMessage = "Please inform the number of collections.")]
    [Range(0,999,ErrorMessage = "The value must be 0 or positive.")]
    public int CollectionsCount { get; set; }
    
    [Display(Name = "Returns")]
    [Required(ErrorMessage = "Report the number of returns made")]
    [Range(0,999, ErrorMessage = "The value must be 0 or positive.")]
    public int RetournsCount { get; set; }
    
    [Display(Name = "Notes / Occurrences")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }
}