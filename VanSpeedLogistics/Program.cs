using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VanSpeedLogistics.Data;
using VanSpeedLogistics.Models;
using VanSpeedLogistics.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1. Adicionando o Contexto ao projeto, configurando para usar MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Adiciona o filtro de exceções de banco de dados
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// >>> CORREÇÃO FINAL 3: ADICIONA O SERVIÇO DE RAZOR PAGES
builder.Services.AddRazorPages(); 

// 3. Configuração do Identity CORRIGIDA 1: Uso de AddIdentity<ApplicationUser, IdentityRole>
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Bloco de Automação do Banco de Dados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    try 
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Executa as instruções das Migrations
        await context.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Alimenta o banco com dados iniciais (Seeding)
        await DbInitializer.SeedRolesAsync(roleManager);
        await DbInitializer.SeedUsersAsync(userManager);

        Console.WriteLine(">>> SUCESSO: Banco de dados sincronizado e Usuários iniciais criados.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, ">>> ERRO: Falha ao inicializar o banco de dados.");
    }
}

// Configure o pipeline de requisição HTTP (Middleware)
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();