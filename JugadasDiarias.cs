using HelloWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWPFApp
{
    public class JugadasDiarias
    {
        LOTOContext db = new LOTOContext();

        public List<int> TicketsDeHoy()
        {
            var date = DateTime.Today;
            var items = db.Ticket.OrderByDescending(a => a.Creado).Where(d => (d.Creado >= date) && (d.Creado <= DateTime.Now)).Select(x=> x.Id).ToList();
            return items;
        }
    }
}
