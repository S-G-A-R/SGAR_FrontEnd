namespace FrontEnd_SGAR.DTOs
{
    public class EmpresaDTO
    {
        public int id { get; set; }
        public int AsociadoId { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
    }

    public class EmpresaRequest
    {
        public int AsociadoId { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
    }
}
