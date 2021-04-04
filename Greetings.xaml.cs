
using HelloWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HelloWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CleanTextBoxes cleanTextBoxes = new CleanTextBoxes();
        ListaDeNumeros listaDeNumeros = new ListaDeNumeros();
        JugadasDiarias jugadasDiarias = new JugadasDiarias();


        public IDictionary<string, int> Jugada_Puntos { get; set; }
        public List<int> ListaDeTickets { get; set; }


        LOTOContext Db = new LOTOContext();
        public List<Loteria> Loteria { get; set; }


        //Regex to only allow numbers on a text box 
        private static readonly Regex _regex = new Regex("[^0-9]+");


        public MainWindow()
        {
            InitializeComponent();
            BindCombo();
        }


        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }


        //Letters not allowed Preview text input
        private void Input_NoLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }


        //After pressing enter on the Puntos or Jugada text boxes, add the values to the dictionary 
        public void AddValuesToDictionary()
        {
            int puntos = cleanTextBoxes.CleanPuntosInput(PuntosInput.Text);
            string jugada = cleanTextBoxes.CleanJugadaInput(JugadaInput.Text);

            if (jugada.Length > 2)
            {
                puntos = 1;
            }
            else if (puntos > 5)
            {
                puntos = 5;
            }
  
            //add puntos and jugada to a dictionary <string, int>
            listaDeNumeros.AddKeyValuePairs(jugada, puntos);

            //add the key (jugada) to jugada table and value (puntos) to ticket_jugada table 
            Jugada_Puntos = this.listaDeNumeros.previewDictionary;
            UpdateItemsOnListaPreviaDeNumeros();
       
            //empty the textboxes
            ProximaJugada();
        }

        //Clean the text boxes of jugada and puntos after the items have been added to the dictionary
        public void ProximaJugada()
        {
            PuntosInput.Clear();
            JugadaInput.Clear();
            PuntosInput.Focus();
        }

        //Update the list view of the dictionary after an item has been added or removed 
        public void UpdateItemsOnListaPreviaDeNumeros()
        {
            VistaPrevia.ItemsSource = Jugada_Puntos;
            VistaPrevia.Items.Refresh();
            VistaPrevia.SelectedIndex = VistaPrevia.Items.Count - 1;
            VistaPrevia.ScrollIntoView(VistaPrevia.SelectedItem);
            int itemsTotal = Jugada_Puntos.Sum(v => v.Value);
            string price = (itemsTotal.ToString() + ".00 € ");
            totalSum.Text = price;
        }

        //Update tickets the hoy list view after a new ticket has been issued (After imprimit button) 
        public void UpdateTicketsDeHoy()
        {
            TicketsDeHoy.ItemsSource = ListaDeTickets;
            TicketsDeHoy.Items.Refresh();
            TicketsDeHoy.SelectedIndex = TicketsDeHoy.Items.Count - 1;
            TicketsDeHoy.ScrollIntoView(TicketsDeHoy.SelectedItem);
        }

        //Combo Box that contains the list of loterias 
        private void BindCombo()
        {
            var loteriaItem = Db.Loteria.ToList();
            Loteria = loteriaItem;
            ListaSeleccionable.DataContext = Loteria;
        }

      
        //Puntos Key Down rule for when pressing Enter on the Puntos text box
        private void PuntosInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(JugadaInput.Text))
            {
                e.Handled = true;
                JugadaInput.Focus();
            }
            else if (e.Key == Key.Enter && !string.IsNullOrEmpty(PuntosInput.Text.Trim('0')))
            {
                AddValuesToDictionary();
            }
        }


        //Jugada Key Down rule for when pressing Enter on the Jugada text box
        private void JugadaInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && string.IsNullOrEmpty(PuntosInput.Text))
            {
                e.Handled = true;
                PuntosInput.Focus();
            }
            else if (e.Key == Key.Enter && !string.IsNullOrEmpty(JugadaInput.Text) && !string.IsNullOrEmpty(PuntosInput.Text.Trim('0')))
            {
                AddValuesToDictionary();
            }
        }

        //Minus Button functionality -- Remove only selected item from the  dictionary
        private void Eliminar_Numero(object sender, RoutedEventArgs e)
        {
            if (this.listaDeNumeros.previewDictionary.Count > 0)
            {
                var keys = string.Join(",", VistaPrevia.SelectedItems.OfType<KeyValuePair<string, int>>().Select(x => x.Key.ToString()));
                Jugada_Puntos.Remove(keys);
                UpdateItemsOnListaPreviaDeNumeros();
            }
        }

        //Cancelar Button functionality -- Remove everything from the dictionary
        private void Borrar_Jugada(object sender, RoutedEventArgs e)
        {
            if (this.listaDeNumeros.previewDictionary.Count>0)
            {
                Jugada_Puntos.Clear();
                UpdateItemsOnListaPreviaDeNumeros();
            }
        }


        // Imprimir button functionality --  Add dictionary to database, and ticket ID to the lista de tickets
        private void BotonImprimir_Click(object sender, RoutedEventArgs e)
        {
            if (this.listaDeNumeros.previewDictionary.Count > 0)
            {
                //get the loteria id from the combobox
                int loteriaSeleccionada = ListaSeleccionable.SelectedIndex + 1;

                //Add all the things on the dictionary to a database table
                listaDeNumeros.AddToDatabase(loteriaSeleccionada);

                //empty the dictionary 
                UpdateItemsOnListaPreviaDeNumeros();

                //Add the new ticket id to the list 
                ListaDeTickets = jugadasDiarias.TicketsDeHoy();
                UpdateTicketsDeHoy();
            }
        }


        //Copiar ticket button functionality 
        private void Copiar_Ticket_Click(object sender, RoutedEventArgs e)
        {
            //create a variable with whatever is on the TicketID text box 
            var ticketID = TicketID.Text;
            //Create a new dictionary that will equal store 
        }


        // REPORTES TAB
        private void ReporteDeVentas_Click(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = Fecha.SelectedDate;
            if (selectedDate.HasValue)
            {
                string formatted = selectedDate.Value.ToString("yyyy-MM-dd");
                var data = Db.Jugada.Where(j => j.Fecha == selectedDate.Value.Date);
                var sum = data.Sum(J => J.Repetido);
                string report = $"En la fecha de {formatted} se vendio un total de {sum} euros";
                Reporte.Text = report;
            }
        }
    }
}
