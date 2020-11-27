using System;
using System.Collections.Generic;

namespace HelloWPFApp.Models
{
    public partial class Loteria
    {
        public Loteria()
        {
            Jugada = new HashSet<Jugada>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Jugada> Jugada { get; set; }
    }
}
