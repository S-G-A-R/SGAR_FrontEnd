namespace FrontEnd_SGAR.DTOs
{
    public class OperadorDTO
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string CodigoOperador { get; set; } = "";
        public string LicenciaDoc { get; set; } = "";
        public int IdOrganizacion { get; set; }
    }
}
