using System;
using System.Collections.Generic;

namespace HelloWPFApp.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            TicketJugada = new HashSet<TicketJugada>();
        }

        public int Id { get; set; }
        public int Pin { get; set; }
        public bool Anulado { get; set; }
        public DateTime? Creado { get; set; }

        public virtual ICollection<TicketJugada> TicketJugada { get; set; }
    }
}
