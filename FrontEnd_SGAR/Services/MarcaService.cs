using Microsoft.JSInterop;
using System.Net.Http.Json;
using static FrontEnd_SGAR.DTOs.MarcaDTO;

namespace FrontEnd_SGAR.Services
{
    public class MarcaService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public MarcaService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AgregarTokenAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Obtener marcas
        public async Task<MarcaResponsePaginada?> ObtenerMarcasAsync(
            int page = 0,
            int size = 10,
            string? nombre = null,
            string? modelo = null,
            string? year = null)
        {
            try
            {
                await AgregarTokenAsync();

                var url = $"ApiAdmi/api/marcas?page={page}&size={size}&sort=id";

                if (!string.IsNullOrEmpty(nombre))
                    url += $"&nombre={Uri.EscapeDataString(nombre)}";

                if (!string.IsNullOrEmpty(modelo))
                    url += $"&modelo={Uri.EscapeDataString(modelo)}";

                if (!string.IsNullOrWhiteSpace(year))
                    url += $"&year={Uri.EscapeDataString(year)}";

                return await _http.GetFromJsonAsync<MarcaResponsePaginada>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MarcaService] Error obtener marcas: {ex.Message}");
                return null;
            }
        }

        // Obtener marca por ID
        public async Task<MarcaDetalleDto?> ObtenerMarcaPorIdAsync(int id)
        {
            try
            {
                await AgregarTokenAsync();
                return await _http.GetFromJsonAsync<MarcaDetalleDto>($"ApiAdmi/api/marcas/{id}");
            }
            catch
            {
                return null;
            }
        }

        // Crear marca
        public async Task<(bool Exito, string Mensaje)> CrearMarcaAsync(MarcaFormModel modelo)
        {
            try
            {
                await AgregarTokenAsync();
                var response = await _http.PostAsJsonAsync("ApiAdmi/api/marcas", modelo);

                if (response.IsSuccessStatusCode)
                    return (true, "Marca creada correctamente");

                var error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        // Editar marca
        public async Task<(bool Exito, string Mensaje)> ActualizarMarcaAsync(int id, MarcaFormModel modelo)
        {
            try
            {
                await AgregarTokenAsync();
                var response = await _http.PutAsJsonAsync($"ApiAdmi/api/marcas/{id}", modelo);

                if (response.IsSuccessStatusCode)
                    return (true, "Marca actualizada correctamente");

                var error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }

        // Eliminar marca
        public async Task<(bool Exito, string Mensaje)> EliminarMarcaAsync(int id)
        {
            try
            {
                await AgregarTokenAsync();
                var response = await _http.DeleteAsync($"ApiAdmi/api/marcas/{id}");

                if (response.IsSuccessStatusCode)
                    return (true, "Marca eliminada correctamente");

                var error = await response.Content.ReadAsStringAsync();
                return (false, error);
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
