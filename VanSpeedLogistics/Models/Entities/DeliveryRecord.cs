using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VanSpeedLogistics.Models.Entities;

/// <summary>
/// Entidade que registra a atividade diária de um operador logístico (driver).
/// </summary>
public class DeliveryRecord
{
    // O Id é a identificaçZao única de cada registro. O [Key] avisa ao Banco de Dados que esta é a chave primária (Primary Key).
    [Key]
    public int Id { get; set; }
    
    
    // A Data de quando o trabalho foi realizado. O DateTime é usado para guardar dia, mês, ano e hora.
    [Required]
    [Display(Name = "Date of Registration")]
    public DateTime Date { get; set; }
    
    // Id do motorista que fez esse registro.
    [Required]
    public string DriverId { get; set;} = string.Empty;
    
    
    
    
    [Required]
    [Range(0, 999)]
    [Display(Name = "Quantity of Deliveries")]
    public int Deliveries { get; set; }
    
    [Required]
    [Range(0, 999)]
    [Display(Name = "Quantity of Collections")]
    public int Collections { get; set; }
    
    
    [Required]
    [Range(0, 999)]
    [Display(Name = "Goods Returned")]
    public int Returns { get; set; }

    [StringLength(500)]
    [Display(Name = "Notes/Observations")]
    public string? Notes { get; set; }
    
    
    [ForeignKey("DriverId")]
    public ApplicationUser? User { get; set; }
    
    public DeliveryRecord()
    {
        this.Date = DateTime.Now;
    }

}