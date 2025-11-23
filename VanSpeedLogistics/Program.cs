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
    .AddEntityFrameworkStores<ApplicationDbContext>();
    // .AddDefaultUI();
    // .AddDefaultTokenProviders(); 

builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    await DbInitializer.SeedRolesAsync(roleManager);
    await DbInitializer.SeedUsersAsync(userManager);
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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();