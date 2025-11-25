using System.ComponentModel.DataAnnotations;

namespace FrontEnd_SGAR.DTOs
{
    public class HorarioDTO
    {
        public int Id { get; set; }
        public string Dia { get; set; } = "";
        public string ZonaId { get; set; } = "";
        public string ZonaNombre { get; set; } = ""; // NUEVO
        public byte Turno { get; set; }
        public int IdOrganizacion { get; set; }
        public string OrganizacionNombre { get; set; } = ""; // NUEVO
        public string HoraEntrada { get; set; } = "";
        public string HoraSalida { get; set; } = "";
    }

    public class FiltroHorario
    {
        public int Organizacion { get; set; }
        public string Dia { get; set; } = "";
        public string ZonaId { get; set; } = "";
        public byte Turno { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }

    public class PaginacionHorarioResponse
    {
        public List<HorarioDTO> Items { get; set; } = new();
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class HorarioGuardarDto
    {
        [Required]
        public string Dia { get; set; } = "";
        [Required]
        public string ZonaId { get; set; } = "";
        [Required]
        public byte Turno { get; set; }
        [Required]
        public int IdOrganizacion { get; set; }

        public string HoraEntrada { get; set; } = "";
        public string HoraSalida { get; set; } = "";
    }

}

