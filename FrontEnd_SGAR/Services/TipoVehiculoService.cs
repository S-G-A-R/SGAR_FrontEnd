using FrontEnd_SGAR.DTOs;
using System.Net.Http.Json;

namespace FrontEnd_SGAR.Services
{
    public class TipoVehiculoService
    {
        private readonly HttpClient _http;

        public TipoVehiculoService(HttpClient http)
        {
            _http = http;
        }

        // ============================
        // LISTAR / FILTRAR
        // ============================
        public async Task<TipoVehiculoDTO.TipoVehiculoResponsePaginada?> ObtenerTiposAsync(
            int page = 0,
            int size = 10,
            byte? tipo = null,
            string? descripcion = null)
        {
            try
            {
                var url = $"ApiAdmi/api/tipos-vehiculos?page={page}&size={size}&sort=id";

                if (tipo.HasValue)
                    url += $"&tipo={tipo.Value}";

                if (!string.IsNullOrWhiteSpace(descripcion))
                    url += $"&descripcion={Uri.EscapeDataString(descripcion)}";

                return await _http.GetFromJsonAsync<TipoVehiculoDTO.TipoVehiculoResponsePaginada>(url);
            }
            catch
            {
                return null;
            }
        }

        // Obtener por ID
        public async Task<TipoVehiculoDTO.TipoVehiculoDetalleDto?> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<TipoVehiculoDTO.TipoVehiculoDetalleDto>(
                    $"ApiAdmi/api/tipos-vehiculos/{id}");
            }
            catch
            {
                return null;
            }
        }

        // Crear
        public async Task<(bool Exito, string Mensaje)> CrearAsync(TipoVehiculoDTO.TipoVehiculoFormModel modelo)
        {
            try
            {
                var response = await _http.PostAsync(
                    $"ApiAdmi/api/tipos-vehiculos?tipo={modelo.Tipo}&descripcion={Uri.EscapeDataString(modelo.Descripcion)}",
                    null);

                if (response.IsSuccessStatusCode)
                    return (true, "Tipo de vehículo creado correctamente");

                return (false, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Actualizar
        public async Task<(bool Exito, string Mensaje)> ActualizarAsync(int id, TipoVehiculoDTO.TipoVehiculoFormModel modelo)
        {
            try
            {
                var response = await _http.PutAsync(
                    $"ApiAdmi/api/tipos-vehiculos/{id}?tipo={modelo.Tipo}&descripcion={Uri.EscapeDataString(modelo.Descripcion)}",
                    null);

                if (response.IsSuccessStatusCode)
                    return (true, "Tipo de vehículo actualizado correctamente");

                return (false, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Eliminar
        public async Task<(bool Exito, string Mensaje)> EliminarAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"ApiAdmi/api/tipos-vehiculos/{id}");

                if (response.IsSuccessStatusCode)
                    return (true, "Tipo de vehículo eliminado correctamente");

                return (false, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Verificar si existe
        public async Task<bool> ExisteAsync(byte tipo)
        {
            try
            {
                return await _http.GetFromJsonAsync<bool>($"ApiAdmi/api/tipos-vehiculos/existe?tipo={tipo}");
            }
            catch
            {
                return false;
            }
        }
    }
}
