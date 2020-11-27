using System;
using System.Collections.Generic;

namespace HelloWPFApp.Models
{
    public partial class Jugada
    {
        public Jugada()
        {
            TicketJugada = new HashSet<TicketJugada>();
        }

        public Guid Id { get; set; }
        public string Numero { get; set; }
        public int Repetido { get; set; }
        public int? LoteriaId { get; set; }

        public virtual Loteria Loteria { get; set; }
        public virtual ICollection<TicketJugada> TicketJugada { get; set; }
    }
}
