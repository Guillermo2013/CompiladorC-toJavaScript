using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores
{
    public class Token
    {
        public TokenTipos Tipo { get; set; }
        public string Lexema { get; set; }
        public int Columna { get; set; }
        public int Fila { get; set; }

        public override string ToString()
        {
            return ("Lexema: " + Lexema + " Tipo: " + Tipo + " Fila:" + Fila + " Columna: " + Columna);
        }
    }
}
