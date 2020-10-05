using HelloWPFApp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWPFApp
{
    public class ListaDeNumeros
    {

        public string jugada;
        public int puntos;
        public IDictionary<string, int> previewDictionary = new Dictionary<string, int>();
        LOTOEntities Db = new LOTOEntities();
        Random generator = new Random();

        public ListaDeNumeros(string jugada, int puntos)
        {
            this.jugada = jugada;
            this.puntos = puntos;
        }
        public ListaDeNumeros()
        {


        }
        public void AddKeyValuePairs(string jugada, int puntos)
        {
            if (previewDictionary.ContainsKey(jugada))
            {
                previewDictionary[jugada] = puntos;
            }
            else
            {
                previewDictionary.Add(jugada, puntos);
            }


        }

        public void AddToDatabase(int loteria)
        {
            int pin = generator.Next(1000000, 10000000);

            Ticket ticket = new Ticket
            {
                PIN = pin,
                Anulado = false
            };

            Db.Tickets.Add(ticket);
            Db.SaveChanges();

            foreach (KeyValuePair<string, int> entry in previewDictionary)
            {
                Jugada jugada = new Jugada
                {
                    Numero = entry.Key,
                    LoteriaId = loteria,
                    Repetido = 0, // this will contain an algorithm jugada_calculation(entry.) if same numero and same loteria are added again --> (increase repetido value)
                    ID = Guid.NewGuid()
                };
                Db.Jugadas.Add(jugada);
                Db.SaveChanges();

                Ticket_Jugada ticketJugada = new Ticket_Jugada
                {
                    TicketID = ticket.ID,
                    JugadaID = jugada.ID,
                    Puntos = entry.Value
                };
                Db.Ticket_Jugada.Add(ticketJugada);
                Db.SaveChanges();
            }

            previewDictionary.Clear();
        }
    }
}
