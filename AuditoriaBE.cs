using System;

namespace BarberiaTurnos.BE
{
    public class AuditoriaBE
    {
        public int IdAuditoria { get; set; }
        public int? IdUsuario { get; set; }
        public string Evento { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
