using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using STS_HELP;
using STS_HELP.Controllers;
using STS_HELP.Data;
using STS_HELP.Helper;
using STS_HELP.Repositorio;




var builder = WebApplication.CreateBuilder(args);

var supabaseUrl = builder.Configuration["Supabase:Url"] + "/auth/v1";
var supabaseKey = builder.Configuration["Supabase:ServiceKey"];


// Add services to the container.
builder.Services.AddControllersWithViews();



//INJEÇÃO DE DEPENDÊNCIA:
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<BancoContext>( o => o.UseNpgsql(builder.Configuration.GetConnectionString("DataBase")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<IChamadoRepositorio, ChamadoRepositorio>();
builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<ISessao, Sessao>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton(provider =>
{
    // O nome da classe é 'ClientOptions', e não 'GotrueClientOptions'
    var gotrueOptions = new Supabase.Gotrue.ClientOptions
    {
        Url = supabaseUrl, // A URL de autenticação do seu appsettings

        // Adiciona os Headers de Admin
        Headers = new Dictionary<string, string>
        {
            { "apikey", supabaseKey }, // A ServiceKey
            { "Authorization", $"Bearer {supabaseKey}" }
        }
    };

    // Retorna o cliente Gotrue (e não o Supabase.Client)
    return new Supabase.Gotrue.Client(gotrueOptions);
});


builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


var caminhoRotativa = System.IO.Path.Combine(app.Environment.WebRootPath, "lib", "rotativa");
Rotativa.AspNetCore.RotativaConfiguration.Setup(caminhoRotativa, string.Empty);


app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();


app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
