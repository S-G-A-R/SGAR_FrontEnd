namespace FrontEnd_SGAR.DTOs
{
    public class VehiculoDTO
    {
        public class MarcaDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = "";
            public string Modelo { get; set; } = "";
        }

        public class TipoVehiculoDto
        {
            public int Id { get; set; }
            public string Descripcion { get; set; } = "";
        }

        public class OperadorDto
        {
            public int Id { get; set; }
            public int IdUser { get; set; }
            public string CodigoOperador { get; set; } = "";
            public int IdOrganizacion { get; set; }
        }

      
    }
}
