using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HelloWPFApp
{
    public class CleanTextBoxes
    {
        public string CleanJugadaInput(string jugada)
        {
            int size = jugada.Length;

            switch (size)
            {
                case 1:
                    jugada = jugada.PadLeft(2, '0');
                    return jugada;
                case 2:
                    return jugada;
                case 3:
                    jugada = jugada.PadLeft(4, '0');
                    return OrderCombinacion(jugada);
                case 4:
                    return OrderCombinacion(jugada);
            }

            throw new System.ArgumentException("Numero, Pale O tripleta no aceptado");
        }

        public int CleanPuntosInput(string text)
        {
            if (Int32.TryParse(text, out int puntos))
            {
                return puntos;
            }
            else
            {
                throw new System.ArgumentException("Los puntos especificados no son validos");
            }
        }

        public string OrderCombinacion(string numeros)
        {
            string[] test = Regex.Split(numeros, "(?<=\\G..)");
            string a = test[0];
            string b = test[1];
            Int32.TryParse(a, out int first);
            Int32.TryParse(b, out int last);
            if (first > last)
            {
                numeros = b + a;
            }
            else
            {
                numeros = a + b;
            }
            return numeros;
        }
    }
}
