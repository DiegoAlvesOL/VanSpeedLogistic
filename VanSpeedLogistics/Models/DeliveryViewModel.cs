using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

/// <summary>
/// ViewModel used to receive data from the delivery log form.
/// Contains validations to ensure data is filled correctly.
/// </summary>
public class DeliveryViewModel
{
    // Corrigido para Deliveries
    [Display(Name = "Completed Deliveries")]
    [Required(ErrorMessage = "Please inform the number of deliveries.")]
    [Range(0, 999, ErrorMessage = "The value must be 0 or positive.")]
    public int Deliveries { get; set; } // Renomeado de DeliveriesCount
    
    // Corrigido para Collections
    [Display(Name = "Collections")]
    [Required(ErrorMessage = "Please inform the number of collections (pickups).")]
    [Range(0, 999, ErrorMessage = "The value must be 0 or positive.")]
    public int Collections { get; set; } // Renomeado de CollectionsCount
    
    // Corrigido para Returns (nome, grafia e remoção de Count)
    [Display(Name = "Returns")]
    [Required(ErrorMessage = "Please report the number of returns made.")]
    [Range(0, 999, ErrorMessage = "The value must be 0 or positive.")]
    public int Returns { get; set; } // Renomeado de RetornosCount/RetournsCount
    
    [Display(Name = "Notes / Occurrences")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }
}