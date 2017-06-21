using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
namespace Compiladores.Arbol.AccesoresNode
{
    class ArrayAccesor:AccesorNode
    {
        public List<ExpressionNode> dimension { get; set; }
        public override TiposBases ValidateSemantic()
        {
            return base.ValidateSemantic();
        }
    }
}
