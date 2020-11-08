using HelloWPFApp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWPFApp
{
    public class JugadasDiarias
    {
        LOTOEntities db = new LOTOEntities();

        public List<int> ticketsDeHoy()
        {
            var date = DateTime.Today;
            var items = db.Tickets.OrderByDescending(a => a.Creado).Where(d => (d.Creado >= date) && (d.Creado <= DateTime.Now)).Select(x=> x.ID).ToList();
            return items;
        }
    }
}
