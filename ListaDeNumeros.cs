using HelloWPFApp.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

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
            //bool canSave = true;
            using (System.Data.Entity.DbContextTransaction dbTran = Db.Database.BeginTransaction())
            {
                try
                {
                    Ticket ticket = new Ticket
                    {
                        PIN = pin,
                        Anulado = false,
                        Creado = DateTime.Now
                    };

                    Db.Tickets.Add(ticket);
                   // Db.SaveChanges();

                    foreach (KeyValuePair<string, int> entry in previewDictionary)
                    {
                        Jugada jugada = new Jugada();
                        //if in the jugada table i already have el numero y la misma loteria 
                        if ((Db.Jugadas.Any(j => j.Numero == entry.Key && j.LoteriaId == loteria)))//If jugada exists
                        {
                            var jugadaQuery = Db.Jugadas.Where(j => j.LoteriaId == loteria && j.Numero == entry.Key).FirstOrDefault();
                            int valorRepetido = jugadaQuery.Repetido;
                            if (valorRepetido + entry.Value > 5)
                            {
                                throw new Exception();
                                //canSave = false;
                                //break;
                            }
                            //solo haz un update de la columna de repetido
                            Db.Jugadas.Where(j => j.LoteriaId == loteria && j.Numero == entry.Key).Update(j => new Jugada { Repetido = j.Repetido + entry.Value });
                            var query = Db.Jugadas.Where(j => j.LoteriaId == loteria && j.Numero == entry.Key).FirstOrDefault();
                            jugada.ID = query.ID;
                        }
                        else //If jugada doesn't exist
                        {
                            jugada.Numero = entry.Key;
                            jugada.LoteriaId = loteria;
                            jugada.Repetido = entry.Value;
                            jugada.ID = Guid.NewGuid();
                            Db.Jugadas.Add(jugada);
                        }


                        Ticket_Jugada ticketJugada = new Ticket_Jugada
                        {
                            TicketID = ticket.ID,
                            JugadaID = jugada.ID,
                            Puntos = entry.Value
                        };
                        Db.Ticket_Jugada.Add(ticketJugada);
                    }

                    //if (canSave)
                    //{
                        Db.SaveChanges();
                        dbTran.Commit();
                        previewDictionary.Clear();
                    //}
                    //else
                    //{
                       // dbTran.Rollback();
                    //}
                }
                catch( Exception e)
                {
                    dbTran.Rollback();
                }
            }

        }
        
    }
}
