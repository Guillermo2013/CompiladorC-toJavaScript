using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
namespace Compiladores.Arbol.BinaryNodes
{
    class NoEsNullNode:BinaryOperatorNode
    {
        public override string GenerarCodigo()
        {
            return "(" + OperadorIzquierdo.GenerarCodigo() + "!=null)?" + OperadorIzquierdo.GenerarCodigo()+":"+ OperadorDerecho.GenerarCodigo();
        }
    }
}
