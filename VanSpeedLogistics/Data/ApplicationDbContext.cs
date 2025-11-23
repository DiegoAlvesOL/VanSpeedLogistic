using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VanSpeedLogistics.Models; 
// 🚨 A LINHA QUE FALTAVA: Importa o namespace onde sua classe DeliveryRecord está.
using VanSpeedLogistics.Models.Entities; 

namespace VanSpeedLogistics.Data;

// O Contexto herda de IdentityDbContext usando sua classe customizada ApplicationUser.
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Seu DbSet: Mapeia a classe DeliveryRecord para a tabela DeliveryRecords no MySQL.
    public DbSet<DeliveryRecord> DeliveryRecords { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Esta chamada é crucial para que as tabelas AspNet* sejam mapeadas.
        base.OnModelCreating(builder);
    }
}