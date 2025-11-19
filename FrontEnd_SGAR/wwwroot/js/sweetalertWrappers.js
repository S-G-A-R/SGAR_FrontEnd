
// Alerta de Éxito (se cierra automáticamente)
window.showSuccessAlert = (message) => {
    Swal.fire({
        icon: 'success',
        title: '¡Éxito!',
        text: message,
        showConfirmButton: false,
        timer: 3000
    });
};

// Alerta de Error (requiere que el usuario haga clic)
window.showErrorAlert = (message) => {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        confirmButtonText: 'Aceptar'
    });
};

// Alerta de Confirmación (reemplaza la función nativa 'confirm')
window.showConfirmationAlert = async (title, text, confirmButtonText = 'Sí, eliminar', cancelButtonText = 'No, cancelar') => {
    const result = await Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DC3545', // Rojo para eliminar
        cancelButtonColor: '#3f51b5', // Azul para cancelar
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText
    });
    return result.isConfirmed; // Devuelve true si el usuario presiona el botón de confirmación
};