using FrontEnd_SGAR;
using FrontEnd_SGAR.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SgarApiVenta.Client.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//  Registrar ILocalStorageService y el Handler
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationHeaderHandler>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//CONEXI�N DE LA API SEGURIDAD
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://sgarseguridad.somee.com/") //API SEGURIDAD
});

// CONEXI�N DE LA API DE NAVEGACI�N
builder.Services.AddHttpClient("NavigationAPI", client =>
{
    client.BaseAddress = new Uri("https://sgar-navigation.onrender.com/");
});

// API JAVA
builder.Services.AddHttpClient("JavaAPI", client =>
{
    client.BaseAddress = new Uri("https://sgar-api-java.onrender.com/");
});

// CONEXIÓN DE LA API VENTAS (PARA ASOCIADOS) 
builder.Services.AddHttpClient("VentasAPI", client =>
{
    client.BaseAddress = new Uri("https://sgar-api-venta.onrender.com/");
})

.AddHttpMessageHandler<AuthenticationHeaderHandler>();


//Servicio de autenticacion
builder.Services.AddScoped<AuthSeguridadService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MunicipioService>();
builder.Services.AddScoped<GOrganizacionService>();

builder.Services.AddScoped<CategoriaProductoService>();

await builder.Build().RunAsync();
