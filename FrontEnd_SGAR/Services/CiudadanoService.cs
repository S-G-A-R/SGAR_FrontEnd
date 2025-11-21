using System.Net.Http.Json;
using static FrontEnd_SGAR.DTOs.CiudadanoDTO;

namespace FrontEnd_SGAR.Services
{
    public class CiudadanoService
    {
        private readonly HttpClient _http;

        public CiudadanoService(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Registra un ciudadano con su zona
        /// </summary>
        public async Task<ResultadoOperacion<Ciudadano>> RegistrarCiudadanoAsync(string idZona, int idUser)
        {
            try
            {
                var data = new
                {
                    idZona,
                    idUser
                };

                var response = await _http.PostAsJsonAsync("api/ciudadano", data);

                if (response.IsSuccessStatusCode)
                {
                    var ciudadano = await response.Content.ReadFromJsonAsync<Ciudadano>();
                    
                    if (ciudadano != null)
                    {
                        Console.WriteLine($"[CiudadanoService] Ciudadano registrado exitosamente - Id: {ciudadano.Id}");
                        
                        // Guardar idCiudadano en localStorage para uso futuro
                        return ResultadoOperacion<Ciudadano>.Exitoso(ciudadano);
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[CiudadanoService] Error al registrar: {errorContent}");
                return ResultadoOperacion<Ciudadano>.Fallido("No se pudo registrar la zona.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CiudadanoService] Excepción: {ex.Message}");
                return ResultadoOperacion<Ciudadano>.Fallido($"Error del sistema: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene ciudadano por idUser (si el endpoint existe)
        /// </summary>
        public async Task<Ciudadano?> ObtenerCiudadanoPorUserIdAsync(int idUser)
        {
            try
            {
                Console.WriteLine($"[CiudadanoService] Obteniendo ciudadano para idUser: {idUser}");
                
                // Nota: Este endpoint debe existir en el backend o usar otra estrategia
                var ciudadano = await _http.GetFromJsonAsync<Ciudadano>($"api/ciudadano/{idUser}");
                
                if (ciudadano != null)
                {
                    Console.WriteLine($"[CiudadanoService] Ciudadano encontrado - Id: {ciudadano.Id}, idUser: {ciudadano.IdUser}");
                }
                
                return ciudadano;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CiudadanoService] Error: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Clase genérica para manejar resultados de operaciones
    /// </summary>
    public class ResultadoOperacion<T>
    {
        public bool EsExitoso { get; set; }
        public string MensajeError { get; set; } = string.Empty;
        public T? Datos { get; set; }

        public static ResultadoOperacion<T> Exitoso(T datos)
        {
            return new ResultadoOperacion<T>
            {
                EsExitoso = true,
                Datos = datos
            };
        }

        public static ResultadoOperacion<T> Fallido(string mensaje)
        {
            return new ResultadoOperacion<T>
            {
                EsExitoso = false,
                MensajeError = mensaje
            };
        }
    }
}
