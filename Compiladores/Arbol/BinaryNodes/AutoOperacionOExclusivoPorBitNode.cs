using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Semantico;
using Compiladores.Semantico.Tipos;
using Compiladores.Arbol.Identificadores;
using Compiladores.Arbol.StatementNodes;
namespace Compiladores.Arbol.BinaryNodes
{
    class AutoOperacionOExclusivoPorBitNode:BinaryOperatorNode
    {

        public override TiposBases ValidateSemantic()
        {

            TiposBases expresion1;
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
            else if (OperadorIzquierdo is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;

                expresion1 = OperadorDerecho.ValidateSemantic();
            if (OperadorIzquierdo == null)
                return expresion1;
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException("no se puede asignar literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion2 = OperadorIzquierdo.ValidateSemantic();
            if (expresion1 is BooleanTipo && expresion2 is BooleanTipo)
                return expresion1;
            if (expresion2 is IntTipo && expresion1 is IntTipo)
                return new IntTipo();
            throw new SemanticoException("no se puede auto operacion o logico no se puede  " + expresion1 + " con " + expresion2 + "fila " + token.Fila + " columna " + token.Columna);
        }
 
    }
}
