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
    public class AsignacionNode:BinaryOperatorNode
    {
        public override TiposBases ValidateSemantic()
        {
            if (OperadorDerecho is CallFuntionNode)
                (OperadorDerecho as CallFuntionNode).claseActual = ContenidoStack._StackInstance.claseActual;
          
            if (!(OperadorIzquierdo is IdentificadoresExpressionNode))
                throw new SemanticoException("no se puede asignar literales  fila " + token.Fila + " columna " + token.Columna);
            var expresion1 = OperadorDerecho.ValidateSemantic();
            if (OperadorIzquierdo == null)
                return expresion1;
            var expresion2 = OperadorDerecho.ValidateSemantic(); 
            if (expresion1.GetType() == expresion2.GetType())
                return expresion2;

            throw new SemanticoException("no se puede asignar" + expresion1 + " con " + expresion2 + " fila " + token.Fila + " columna " + token.Columna);
        }
    }
}
