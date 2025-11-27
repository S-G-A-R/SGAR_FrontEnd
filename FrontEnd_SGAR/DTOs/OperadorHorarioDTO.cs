namespace FrontEnd_SGAR.DTOs
{
    public class OperadorHorarioDTO
    {
        public int Id { get; set; }

        public int OperadorId { get; set; }
        public string CodigoOperador { get; set; } = ""; // viene de operador

        public List<HorarioDTO> Horarios { get; set; } = new();
        public List<int> HorariosIds { get; set; } = new(); // para POST/PUT
    }
}