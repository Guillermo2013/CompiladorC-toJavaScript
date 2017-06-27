
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiladores.Semantico.Tipos
{
    public class ArrayTipo:TiposBases
    {
        public TiposBases tipoArray;
        public List <List<int>> cantidad = new List<List<int>>();
        
    }
}
