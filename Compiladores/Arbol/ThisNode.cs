using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
namespace Compiladores.Arbol
{
    class ThisNode:ExpressionNode
    {
        public ExpressionNode expresion;
        public override Semantico.TiposBases ValidateSemantic()
        {
            throw new NotImplementedException();
        }
    }
}
