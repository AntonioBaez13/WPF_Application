//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HelloWPFApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket_Jugada
    {
        public int TicketID { get; set; }
        public int JugadaID { get; set; }
        public int Puntos { get; set; }
    
        public virtual Jugada Jugada { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
