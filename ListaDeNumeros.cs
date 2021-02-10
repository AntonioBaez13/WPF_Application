using HelloWPFApp.Models;
using Microsoft.EntityFrameworkCore.Storage;
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
        LOTOContext Db = new LOTOContext();
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
            

            using (IDbContextTransaction dbTran = Db.Database.BeginTransaction())
            {
                try
                {
                    Ticket ticket = new Ticket
                    {
                        Pin = pin,
                        Anulado = false,
                        Creado = DateTime.Now
                    };

                    Db.Ticket.Add(ticket);
                    Db.SaveChanges();

                    foreach (KeyValuePair<string, int> entry in previewDictionary)
                    {
                        Jugada jugada = new Jugada();
                        //if in the jugada table i already have el numero y la misma loteria y PARA LA MISMA FECHA
                        if ((Db.Jugada.Any(j => j.Numero == entry.Key 
                                            && j.LoteriaId == loteria 
                                            && j.Fecha == DateTime.UtcNow.Date)))//If jugada exists
                        {
                            //if the jugada length is 4 (pale) then throw the exception without checking anything
                            if(entry.Key.Length > 2)
                            {
                                throw new Exception();
                            }
                            var jugadaQuery = Db.Jugada.Where(j => j.LoteriaId == loteria 
                                                                && j.Numero == entry.Key 
                                                                && j.Fecha == DateTime.UtcNow.Date).FirstOrDefault();
                            int valorRepetido = jugadaQuery.Repetido;
                            if (valorRepetido + entry.Value > 5)
                            {
                                throw new Exception();
                                //TODO show a message with the wrong values
                            }
                            //solo haz un update de la columna de repetido
                            var valor = entry.Value;
                            Db.Jugada.Where(j => j.LoteriaId == loteria 
                                            && j.Numero == entry.Key 
                                            && j.Fecha == DateTime.UtcNow.Date).Update(j => new Jugada { Repetido = j.Repetido + valor });
                            var query = Db.Jugada.Where(j => j.LoteriaId == loteria 
                                                        && j.Numero == entry.Key 
                                                        && j.Fecha == DateTime.UtcNow.Date).FirstOrDefault();
                            jugada.Id = query.Id;
                        }
                        else //If jugada doesn't exist
                        {
                            jugada.Numero = entry.Key;
                            jugada.LoteriaId = loteria;
                            jugada.Repetido = entry.Value;
                            jugada.Fecha = DateTime.UtcNow.Date;
                            jugada.Id = Guid.NewGuid();
                            Db.Jugada.Add(jugada);
                        }


                        TicketJugada ticketJugada = new TicketJugada
                        {
                            TicketId = ticket.Id,
                            JugadaId = jugada.Id,
                            Puntos = entry.Value
                        };
                        Db.TicketJugada.Add(ticketJugada);
                    }
                    Db.SaveChanges();
                    PrintReceipt print = new PrintReceipt(previewDictionary, pin, ticket.Id, loteria);
                    print.PrintTicket();
                    dbTran.Commit();
                    previewDictionary.Clear();
                   
                }
                catch( Exception e)
                {
                    dbTran.Rollback();
                }
            }

        }
        
    }
}
