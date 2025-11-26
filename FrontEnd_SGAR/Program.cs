using FrontEnd_SGAR;
using FrontEnd_SGAR.Handlers;
using FrontEnd_SGAR.Providers;
using FrontEnd_SGAR.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Registrar servicios de autenticación y autorización
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();

// Registrar el AuthenticationHandler como servicio transient
builder.Services.AddTransient<AuthenticationHandler>();

// CONEXIÓN ÚNICA AL API GATEWAY - HttpClient por defecto
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthenticationHandler>();
    handler.InnerHandler = new HttpClientHandler();

    return new HttpClient(handler)
    {
        BaseAddress = new Uri("https://gatewaysgar.onrender.com/")
    };
});

//Servicios de autenticacion y otros
builder.Services.AddScoped<AuthSeguridadService>();
builder.Services.AddScoped<AuthOrganizacionService>();
builder.Services.AddScoped<MunicipioService>();
builder.Services.AddScoped<ZonaService>();
builder.Services.AddScoped<OrganizacionService>();
builder.Services.AddScoped<CiudadanoService>();
builder.Services.AddScoped<TipoSuscripcionService>();
builder.Services.AddScoped<VehiculoServices>();
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<MarcaService>();
builder.Services.AddScoped<TipoVehiculoService>();
builder.Services.AddScoped<HorarioService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<AdministradorService>();

await builder.Build().RunAsync();
