namespace FrontEnd_SGAR.DTOs
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CategoriaProductoId { get; set; }
        public int EmpresaId { get; set; }
        public string ImagenBase64 { get; set; } = string.Empty;
    }
}
