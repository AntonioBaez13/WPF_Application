//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HelloWPFApp.Db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            this.Ticket_Jugada = new HashSet<Ticket_Jugada>();
        }
    
        public int ID { get; set; }
        public int PIN { get; set; }
        public bool Anulado { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket_Jugada> Ticket_Jugada { get; set; }
    }
}
