using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models;

public class DeliveryViewModel
{
    [Display(Name = "Entregas")]
    [Required(ErrorMessage = "Informe a quantidade de entregas realizadas")]
    [Range(0,999,ErrorMessage = "O Valor deve ser positivo.")]
    public int DeliveriesCount { get; set; }
    
    [Display(Name = "Coletas")]
    [Required(ErrorMessage = "Informe a quantidade de coletas realizadas")]
    [Range(0,999,ErrorMessage = "O valor deve ser 0 ou positivo.")]
    public int CollectionsCount { get; set; }
    
    [Display(Name = "Retorno")]
    [Required(ErrorMessage = "Informe a quantidade de retornos realizadas")]
    [Range(0,999, ErrorMessage = "O valor deve ser 0 ou positivo.")]
    public int RetournsCount { get; set; }
    
    [Display(Name = "Observações/Ocorrências")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

}