using GerenciadorDeEmpresaWEB.Services;
using GerenciadorDeEmpresaWEB.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);
var ApiUri = builder.Configuration["ServicoUri:APIGerenciadorDeEmpresas"];

// Add services to the container.
builder.Services.AddHttpClient("APIGerenciadorDeEmpresas", c =>
{
    c.BaseAddress = new Uri(ApiUri);
});

builder.Services.AddScoped<IEmpresasServices, EmpresasServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();
builder.Services.AddScoped<IPerfilUsuarioServices, PerfilUsuarioServices>();
builder.Services.AddScoped<ITipoEmpresaServices, TipoEmpresaServices>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
