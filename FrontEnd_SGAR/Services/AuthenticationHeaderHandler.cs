using Blazored.LocalStorage;
using System.Net.Http.Headers;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthenticationHeaderHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Obtener el token del almacenamiento local.
        
        var token = await _localStorage.GetItemAsync<string>("token"); 

        if (!string.IsNullOrEmpty(token))
        {
            // 2. Adjuntar el token al encabezado "Authorization"
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}