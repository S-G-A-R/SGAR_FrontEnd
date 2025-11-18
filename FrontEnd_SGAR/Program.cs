using FrontEnd_SGAR;
using FrontEnd_SGAR.Services;
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


//CONEXIÓN DE LA API SEGURIDAD
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://sgarseguridad.somee.com/") //API SEGURIDAD
});

// CONEXIÓN DE LA API DE NAVEGACIÓN
builder.Services.AddHttpClient("NavigationAPI", client =>
{
    client.BaseAddress = new Uri("https://sgar-navigation.onrender.com/api-docs/");
});

// API JAVA
builder.Services.AddHttpClient("JavaAPI", client =>
{
    client.BaseAddress = new Uri("https://sgar-api-java.onrender.com/");
});



//Servicio de autenticacion
builder.Services.AddScoped<AuthSeguridadService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MunicipioService>();
builder.Services.AddScoped<GOrganizacionService>();


await builder.Build().RunAsync();
