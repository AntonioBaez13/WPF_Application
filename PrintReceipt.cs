using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace HelloWPFApp
{
    public class PrintReceipt
    {
        private IDictionary<string, int> dictionary = new Dictionary<string, int>();
        private int pin;
        private int ticketID;
        private string loteriaID;

        
        public PrintReceipt(IDictionary<string, int> previewDictionary, int pin, int ticket, int loteria)
        {
            this.dictionary = previewDictionary;
            this.pin = pin;
            this.ticketID = ticket;
            this.loteriaID = GetLoteriaName(loteria);
        }

        public string GetLoteriaName(int loteria)
        {
            IDictionary<int, string> idToName = new Dictionary<int, string>
            {
                { 1, "Nacional Noche" },
                { 2, "Quiniela Pale" },
                { 3, "Nacional Tarde" },
                { 4, "Real" },
                { 5, "Nueva York Dia" },
                { 6, "Nueva York Noche" },
                { 7, "Loteka" },
                { 8, "La Primera" },
                { 9, "Florida Dia" },
                { 10, "Florida Noche" },
                { 11, "Georgia Dia" },
                { 12, "Georgia Noche" }
            };
            string name = idToName[loteria];
            return name;
        }

        public void PrintTicket()
        {
            PrintDialog printDialog = new PrintDialog();

            PrintDocument printDocument = new PrintDocument();

            printDialog.Document = printDocument;

            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);

            DialogResult result = printDialog.ShowDialog();

            if(result == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 12);
            Font bigFont = new Font("Courier New", 18);
            Brush brush = new SolidBrush(Color.Black);
            float fontHeight = font.GetHeight();
            float bigFontHeight = bigFont.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset1 = 30;

            graphic.DrawString("3030", new Font("Courier New", 24, FontStyle.Bold), brush, startX, startY);
            graphic.DrawString(loteriaID, new Font("Courier New", 20, FontStyle.Bold), brush, startX, startY + offset1);
            offset1 = offset1 + (int)bigFontHeight + 5;
            graphic.DrawString(("Ticket: " + ticketID.ToString()), font, brush, startX, startY + offset1);
            offset1 = offset1 + (int)fontHeight + 5;
            graphic.DrawString(("Pin: " + pin.ToString()), font, brush, startX, startY + offset1);
            offset1 = offset1 + (int)fontHeight + 5;
            graphic.DrawString((DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")), font, brush, startX, startY + offset1);
            offset1 = offset1 + (int)fontHeight + 5;
            graphic.DrawString("--------------------------------------", font, brush, startX, startY + offset1);
            offset1 = offset1 + (int)fontHeight + 5;

            foreach (KeyValuePair<string, int> entry in dictionary)
            {
                string numeroJugado = entry.Key.PadRight(10);
                string puntosJugada =entry.Value.ToString() + ".00 € ";
                string jugadaLine = numeroJugado + puntosJugada;

                graphic.DrawString(jugadaLine, new Font("Courier New", 18, FontStyle.Bold), brush, startX, startY + offset1);

                offset1 = offset1 + (int)bigFontHeight + 5;
            }

            int itemsTotal = dictionary.Sum(v => v.Value);
            graphic.DrawString("--------------------------------------", font, brush, startX, startY + offset1);
            offset1 = offset1 + 12;
            graphic.DrawString(("Total " + itemsTotal.ToString() + ".00 € "), new Font("Courier New", 18, FontStyle.Bold), brush, startX, startY + offset1);
        }
    }
}