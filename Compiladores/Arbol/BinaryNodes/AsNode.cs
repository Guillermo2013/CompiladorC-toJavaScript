using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiladores.Arbol.BaseNode;
using Compiladores.Arbol.Identificadores;
using Compiladores.Semantico;

namespace Compiladores.Arbol.BinaryNodes
{
    class AsNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException("no se puede castear literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion1 = OperadorIzquierdo.ValidateSemantic();
            var expresion2 = OperadorDerecho.ValidateSemantic();
            return expresion2;
        }
    }
}
