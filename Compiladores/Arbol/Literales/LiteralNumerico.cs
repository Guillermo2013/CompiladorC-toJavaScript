using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.Literales
{
    public class LiteralNumerico:ExpressionNode
    {
        public int valor { get; set; }
        public override Semantico.TiposBases ValidateSemantic()
        {
            return new IntTipo();
        }
    }
}
