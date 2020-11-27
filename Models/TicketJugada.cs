using System;
using System.Collections.Generic;

namespace HelloWPFApp.Models
{
    public partial class TicketJugada
    {
        public int TicketId { get; set; }
        public Guid JugadaId { get; set; }
        public int Puntos { get; set; }

        public virtual Jugada Jugada { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
