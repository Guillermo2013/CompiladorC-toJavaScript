using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
namespace Compiladores.Arbol.BinaryNodes
{
    class OPorBitNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

            var expresion1 = OperadorDerecho.ValidateSemantic();
            var expresion2 = OperadorIzquierdo.ValidateSemantic();
            if (expresion1 is BooleanTipo && expresion2 is BooleanTipo)
                return expresion1;
            if ((expresion1 is BooleanTipo || expresion2 is IntTipo) && (expresion2 is BooleanTipo || expresion1 is IntTipo))
                return new IntTipo();
            throw new SemanticoException("no se puede auto operacion o logico no se puede  " + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
    }
}
