using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.Literales
{
    class LiteralesFloat:ExpressionNode
    {
        public float valor { get; set; }
        public override Semantico.TiposBases ValidateSemantic()
        {
            return new FloatTipo();
        }
        public override string GenerarCodigo()
        {
            return valor.ToString().Replace("f","");
        }
       
    }
}
