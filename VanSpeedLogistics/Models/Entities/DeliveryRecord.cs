using System.ComponentModel.DataAnnotations;

namespace VanSpeedLogistics.Models.Entities;

/// <summary>
/// Esta classe representa o Model do formulário diário.
/// Cada propriedade aqui vai virar um coluna da tabela do MySQL.
/// </summary>
public class DeliveryRecord
{
    // O Id é a identificaçZao única de cada registro.
    // O [Key] avisa ao Banco de Dados que esta é a chave primária (Primary Key).
    [Key]
    public int Id { get; set; }
    
    // A Data de quando o trabalho foi realizado.
    // O DateTime é usado para guardar dia, mês, ano e hora.
    public DateTime Date { get; set; }
    
    // Id do motorista que fez esse registro.
    public string DriverId { get; set;}
    
    // Quantidade de mercadorias entregues com sucesso.
    public int DeliveriesCount { get; set; }
    
    // Quantidade de mercadorias recolhidas (Coletas).
    public int CollectionsCount { get; set; }
    
    // Quantidade de entregas que falharam (Retornos).
    public int RetournsCount {  get; set; }
    
    // Campo opcional para observações (ex: "Pneu furou", "Trânsito intenso").
    // O ponto de interrogação (string?) indica que aceita ficar vazio (Nulo).
    public string? Notes { get; set; }

    public DeliveryRecord()
    {
        this.Date = DateTime.Now;
    }

}