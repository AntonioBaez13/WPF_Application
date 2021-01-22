
using HelloWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public IDictionary<string, int> X { get; set; }
        public List<int> Y { get; set; }
        LOTOContext Db = new LOTOContext();

        public MainWindow()
        {
            InitializeComponent();
            BindCombo();
        }

        public List<Loteria> Loteria { get; set; }

        private void BindCombo()
        {
            var loteriaItem = Db.Loteria.ToList();
            Loteria = loteriaItem;
            ListaSeleccionable.DataContext = Loteria;
        }

        //Regex for the Preview Input Text rule
        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void Input_NoLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        //Puntos Key Down rule for when pressing Enter
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

        //Jugada Key Down rule for when pressing Enter
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

        //Remove only one item from the  dictionary
        private void Eliminar_Numero(object sender, RoutedEventArgs e)
        {
            var keys = string.Join(",", VistaPrevia.SelectedItems.OfType<KeyValuePair<string, int>>().Select(x => x.Key.ToString()));
            X.Remove(keys);
            UpdateItemsOnListaPreviaDeNumeros();

        }

        private void Borrar_Jugada(object sender, RoutedEventArgs e)
        {
            X.Clear();
            UpdateItemsOnListaPreviaDeNumeros();
        }

        public void AddValuesToDictionary()
        {

            int puntos = cleanTextBoxes.CleanPuntosInput(PuntosInput.Text);
            string jugada = cleanTextBoxes.CleanJugadaInput(JugadaInput.Text);

            if (puntos > 5)
            {
                puntos = 5;
            }

            //add puntos and jugada to a dictionary <string, int>
            listaDeNumeros.AddKeyValuePairs(jugada, puntos);
            
            //add the key (jugada) to jugada table and value (puntos) to ticket_jugada table 
            X = this.listaDeNumeros.previewDictionary;
            UpdateItemsOnListaPreviaDeNumeros();
            
            //empty the textboxes
            ProximaJugada();
        }

        public void ProximaJugada()
        {
            PuntosInput.Clear();
            JugadaInput.Clear();
            PuntosInput.Focus();
        }


        public void UpdateItemsOnListaPreviaDeNumeros()
        {
            VistaPrevia.ItemsSource = X;
            VistaPrevia.Items.Refresh();
            VistaPrevia.SelectedIndex = VistaPrevia.Items.Count - 1;
            VistaPrevia.ScrollIntoView(VistaPrevia.SelectedItem);
            int itemsTotal = X.Sum(v => v.Value);
            string price = (itemsTotal.ToString() + ".00 € ");
            totalSum.Text = price;
        }

        public void UpdateTicketsDeHoy()
        {
            TicketsDeHoy.ItemsSource = Y;
            TicketsDeHoy.Items.Refresh();
        }

        private void BotonImprimir_Click(object sender, RoutedEventArgs e)
        {
            //get the loteria id from the combobox
            int selection = ListaSeleccionable.SelectedIndex + 1;
             
            //Add all the things on the dictionary to a database table
            listaDeNumeros.AddToDatabase(selection);

            //empty the dictionary 
            UpdateItemsOnListaPreviaDeNumeros();
            //Add the new ticket id to the list 
            Y = jugadasDiarias.TicketsDeHoy();
            UpdateTicketsDeHoy();
        }

        public void ShowMessag(string jugada, int total)
        {
            MessageBox.Show($"No hay disponibilidad de la jugada {jugada}, recuerde que el maximo son 5€ y esa jugada ya tiene {total}€ jugados");
        }

        private void Ventas_Click(object sender, RoutedEventArgs e)
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
