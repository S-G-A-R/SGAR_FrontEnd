using Microsoft.JSInterop;

namespace FrontEnd_SGAR.Services
{
    /// <summary>
    /// Servicio para mostrar alertas personalizadas en la UI
    /// </summary>
    public class AlertService
    {
        private readonly IJSRuntime _js;

        public AlertService(IJSRuntime js)
        {
            _js = js;
        }

        /// <summary>
        /// Muestra una alerta de éxito
        /// </summary>
        public async Task MostrarExitoAsync(string mensaje)
        {
            await _js.InvokeVoidAsync("showCustomAlert", mensaje);
        }

        /// <summary>
        /// Muestra una alerta de error
        /// </summary>
        public async Task MostrarErrorAsync(string mensaje)
        {
            await _js.InvokeVoidAsync("showErrorAlert", mensaje);
        }

        /// <summary>
        /// Muestra una alerta de información
        /// </summary>
        public async Task MostrarInfoAsync(string mensaje)
        {
            await _js.InvokeVoidAsync("showInfoAlert", mensaje);
        }

        /// <summary>
        /// Muestra una alerta de advertencia
        /// </summary>
        public async Task MostrarAdvertenciaAsync(string mensaje)
        {
            await _js.InvokeVoidAsync("showWarningAlert", mensaje);
        }
    }
}
