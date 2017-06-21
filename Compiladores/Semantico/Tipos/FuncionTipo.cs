using Compiladores.Arbol;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;

namespace Compiladores.Semantico.Tipos
{
    public class FuncionTipo:TiposBases
    {
        public Dictionary<string, TiposBases> listaParametros = new Dictionary<string, TiposBases>();
        public TiposBases retorno;
      
      
    }
}
