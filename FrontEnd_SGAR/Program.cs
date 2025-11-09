using FrontEnd_SGAR;
using FrontEnd_SGAR.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://sgarseguridad.somee.com/") //API SEGURIDAD
});

//Servicio de autenticacion
builder.Services.AddScoped<AuthSeguridadService>();


await builder.Build().RunAsync();
